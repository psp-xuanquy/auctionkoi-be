using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid.FixedPriceBid
{
    public class FixedPriceAuctionHandler : BaseAuctionHandler, IRequestHandler<PlaceFixedPriceBidCommand, Unit>
    {
        public FixedPriceAuctionHandler(
            ICurrentUserService currentUserService,
            IKoiRepository koiRepository,
            IBidRepository bidRepository,
            IUserRepository userRepository)
            : base(koiRepository, bidRepository, userRepository, currentUserService)
        {
        }

        public async Task<Unit> Handle(PlaceFixedPriceBidCommand request, CancellationToken cancellationToken)
        {
            var bidder = await GetCurrentBidder(cancellationToken);
            var koi = await GetKoiForAuction(request.KoiId, "Fixed Price Sale", cancellationToken);

            await ValidateBid(request, bidder, koi, cancellationToken);
            var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice);
            _bidRepository.Add(bid);

            if (koi.AuctionStatus == AuctionStatus.NotStarted)
            {
                koi.StartAuction();
                _koiRepository.Update(koi);
            }

            // Handle auction expiry and winning bid logic
            await HandleAuctionExpiry(koi, cancellationToken);

            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateBid(PlaceFixedPriceBidCommand request, UserEntity bidder, KoiEntity koi, CancellationToken cancellationToken)
        {
            if (request.BidAmount != koi.InitialPrice)
                throw new InvalidOperationException("Bid amount must be equal to the initial price.");

            if (request.BidAmount > bidder.Balance)
                throw new InvalidOperationException("Bid amount cannot exceed the user's balance.");

            ValidateBidAmount(request.BidAmount, koi, bidder);

            var existingBid = await _bidRepository.GetUserBidForKoi(bidder.Id, request.KoiId);
            if (existingBid != null)
                throw new InvalidOperationException("You have already placed a bid for this auction.");
        }

        private async Task HandleAuctionExpiry(KoiEntity koi, CancellationToken cancellationToken)
        {
            if (koi.IsAuctionExpired())
            {
                var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
                if (bids.Count() > 0)
                {
                    MarkWinningBid(bids);
                }

                koi.EndAuction();
                _koiRepository.Update(koi);
                await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                throw new InvalidOperationException("The auction has already expired.");
            }
        }

        private void MarkWinningBid(IEnumerable<BidEntity> bids)
        {
            if (bids.Count() > 1)
            {
                var random = new Random();
                var winningBid = bids.ElementAt(random.Next(bids.Count()));
                winningBid.MarkAsWinningBid();
                _bidRepository.Update(winningBid);
            }
            else if (bids.Count() == 1)
            {
                var bid = bids.First();
                bid.MarkAsWinningBid();
                _bidRepository.Update(bid);
            }
        }
    }
}
