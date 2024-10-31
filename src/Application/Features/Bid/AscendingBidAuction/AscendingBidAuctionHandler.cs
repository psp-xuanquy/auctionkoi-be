using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Bid.FixedPriceBid;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.AscendingBidAuction;
public class AscendingBidAuctionHandler : BaseAuctionHandler, IRequestHandler<AscendingBidAuctionCommand, Unit>
{
    public AscendingBidAuctionHandler(
        IKoiRepository koiRepository, 
        IBidRepository bidRepository, 
        IUserRepository userRepository, 
        ICurrentUserService currentUserService) 
        : base(koiRepository, bidRepository, userRepository, currentUserService)
    {
    }

    public async Task<Unit> Handle(AscendingBidAuctionCommand request, CancellationToken cancellationToken)
    {
        var bidder = await GetCurrentBidder(cancellationToken);
        var koi = await GetKoiForAuction(request.KoiId, "Ascending Bid Auction", cancellationToken);

        await ValidateBid(request, bidder, koi, cancellationToken);

        var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
        if (bids.Count() > 0)
        {
            foreach (var b in bids)
            {
                b.IsWinningBid = false;
            }
        }

        var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice);
        bid.IsWinningBid = true;

        _bidRepository.Add(bid);

        if (koi.AuctionStatus == AuctionStatus.NotStarted)
        {
            koi.StartAuction();
            _koiRepository.Update(koi);
        }

        await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    private async Task ValidateBid(AscendingBidAuctionCommand request, UserEntity bidder, KoiEntity koi, CancellationToken cancellationToken)
    {
        if (request.BidAmount > bidder.Balance)
            throw new InvalidOperationException("Bid amount cannot exceed the user's balance.");

        ValidateBidAmount(request.BidAmount, koi, bidder);

        var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
        if (bids.Count() > 0)
        {
            foreach (var bid in bids)
            {
                if (bid.BidAmount >= request.BidAmount)
                {
                    throw new InvalidOperationException("Bid amount cannot be lower than or equal the winning bid.");
                }
            }
        }

        var winningBid = bids.Where(b => b.IsWinningBid).FirstOrDefault();
        if (winningBid != null)
        {
            if (winningBid.BidderID == bidder.Id)
            {
                throw new InvalidOperationException("You are currently the winning bidder.");
            }
        }

        if (koi.IsAuctionExpired())
        {
            throw new InvalidOperationException("The auction has already expired.");
        }
    }
}
