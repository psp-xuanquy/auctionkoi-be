using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.SealedBidAuction;
public class PlaceSealedBidAuctionHandler : BaseAuctionHandler, IRequestHandler<PlaceSealedBidAuctionCommand, Unit>
{
    public PlaceSealedBidAuctionHandler(
        ICurrentUserService currentUserService,
        IKoiRepository koiRepository,
        IBidRepository bidRepository,
        IUserRepository userRepository)
        : base(koiRepository, bidRepository, userRepository, currentUserService)
    {
    }

    public async Task<Unit> Handle(PlaceSealedBidAuctionCommand request, CancellationToken cancellationToken)
    {
        var bidder = await GetCurrentBidder(cancellationToken);
        var koi = await GetKoiForAuction(request.KoiId, "Sealed Bid Auction", cancellationToken);

        ValidateBidAmount(request.BidAmount, koi, bidder);

        var existingBid = await _bidRepository.GetUserBidForKoi(bidder.Id, request.KoiId);
        if (existingBid != null)
            throw new InvalidOperationException("You have already placed a bid for this auction.");

        var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice);
        _bidRepository.Add(bid);

        if (koi.AuctionStatus == AuctionStatus.NotStarted)
        {
            koi.StartAuction();
            _koiRepository.Update(koi);
        }

        if (koi.IsAuctionExpired())
        {
            koi.EndAuction();
            _koiRepository.Update(koi);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            throw new InvalidOperationException("The auction has already expired.");
        }

        await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
