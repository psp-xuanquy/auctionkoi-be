using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.SealedBidAuction
{
    public class PlaceSealedBidAuctionHandler : BaseAuctionHandler, IRequestHandler<PlaceSealedBidAuctionCommand, string>
    {
        private readonly IEmailService _emailService;

        public PlaceSealedBidAuctionHandler(ICurrentUserService currentUserService, IKoiRepository koiRepository, IBidRepository bidRepository, IUserRepository userRepository, IEmailService emailService)
            : base(koiRepository, bidRepository, userRepository, currentUserService)
        {
            _emailService = emailService;
        }

        public async Task<string> Handle(PlaceSealedBidAuctionCommand request, CancellationToken cancellationToken)
        {
            var bidder = await GetCurrentBidder(cancellationToken);
            var koi = await GetKoiForAuction(request.KoiId, "Sealed Bid Auction", cancellationToken);

            var existingBid = await _bidRepository.GetUserBidForKoi(koi.ID, bidder.Id, cancellationToken);
            if (existingBid != null)
                throw new BadRequestException("You have already placed a bid for this auction.");

            await ValidateBid(request.BidAmount, koi.ID, bidder.Id, cancellationToken);

            bidder.Balance -= request.BidAmount;
            _userRepository.Update(bidder);

            var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice);
            _bidRepository.Add(bid);

            if (koi.AuctionStatus == AuctionStatus.NotStarted || koi.AuctionStatus == AuctionStatus.OnGoing)
            {
                koi.StartAuction();
                _koiRepository.Update(koi);
            }

            await HandleAuctionExpiry(koi, cancellationToken);
            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return $"You have successfully bid {request.BidAmount} for the fish {koi.Name}.";
        }

        private async Task HandleAuctionExpiry(KoiEntity koi, CancellationToken cancellationToken)
        {
            if (!koi.IsAuctionExpired()) return;

            var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
            if (bids.Any())
            {
                var winningBid = bids.OrderByDescending(b => b.BidAmount).First();
                await _emailService.SendWinningEmail(winningBid.Bidder.Email, koi.Name, winningBid.BidAmount);
            }
            else
            {
                foreach (var bid in bids)
                {
                    var bidder = await _userRepository.FindAsync(x => x.Id == bid.BidderID, cancellationToken);
                    bidder.Balance += bid.BidAmount;
                    _userRepository.Update(bidder);
                }
            }

            koi.EndAuction();
            _koiRepository.Update(koi);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task RefundBidders(IEnumerable<BidEntity> bids, IUserRepository userRepository, CancellationToken cancellationToken)
        {
            foreach (var bid in bids)
            {
                var bidder = await userRepository.FindAsync(x => x.Id == bid.BidderID, cancellationToken);
                if (bidder != null)
                {
                    bidder.Balance += bid.BidAmount;
                    userRepository.Update(bidder);
                }
            }
        }

        private async Task ValidateBid(decimal bidAmount, string koiId, string bidderId, CancellationToken cancellationToken)
        {
            var koi = await _koiRepository.FindAsync(k => k.ID == koiId, cancellationToken);
            if (bidAmount < koi.InitialPrice)
                throw new BadRequestException($"Bid amount must be greater than the starting price of {koi.InitialPrice:C}.");

            await ValidateBidAmount(bidAmount, koiId, bidderId, cancellationToken);
        }
    }
}
