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

namespace Application.Features.Bid.FixedPriceBid
{
    public class FixedPriceAuctionHandler : BaseAuctionHandler, IRequestHandler<PlaceFixedPriceBidCommand, string>
    {
        private readonly IEmailService _emailService;

        public FixedPriceAuctionHandler(ICurrentUserService currentUserService, IKoiRepository koiRepository, IBidRepository bidRepository, IUserRepository userRepository, IEmailService emailService)
            : base(koiRepository, bidRepository, userRepository, currentUserService)
        {
            _emailService = emailService;
        }

        public async Task<string> Handle(PlaceFixedPriceBidCommand request, CancellationToken cancellationToken)
        {
            var bidder = await GetCurrentBidder(cancellationToken);
            var koi = await GetKoiForAuction(request.KoiId, "Fixed Price Sale", cancellationToken);

            var existingBid = await _bidRepository.GetUserBidForKoi(koi.ID, bidder.Id, cancellationToken);
            if (existingBid != null)
                throw new Exception("You have already placed a bid for this auction.");

            await ValidateBid(request.BidAmount, koi.ID, bidder.Id, cancellationToken);

            if (koi.IsAuctionExpired())
            {
                await HandleAuctionExpiry(koi, cancellationToken);
                throw new Exception("The auction has already expired.");
            }

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
                var winningBid = MarkWinningBid(bids);
                await _emailService.SendWinningEmail(winningBid.Bidder.Email, koi.Name, winningBid.BidAmount);
            }
            else
            {
                // Hoàn tiền cho người đã đặt cược
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

        private async Task ValidateBid(decimal bidAmount, string koiId, string bidderId, CancellationToken cancellationToken)
        {
            var koi = await _koiRepository.FindAsync(k => k.ID == koiId, cancellationToken);
            if (bidAmount != koi.InitialPrice)
                throw new BadRequestException("Bid amount must be equal to the initial price.");

            await ValidateBidAmount(bidAmount, koiId, bidderId, cancellationToken);
        }

        private BidEntity MarkWinningBid(IEnumerable<BidEntity> bids)
        {
            BidEntity winningBid = null;
            if (bids.Count() > 1)
            {
                var random = new Random();
                winningBid = bids.ElementAt(random.Next(bids.Count()));
            }
            else if (bids.Count() == 1)
            {
                winningBid = bids.First();
            }

            if (winningBid != null)
            {
                winningBid.MarkAsWinningBid();
                _bidRepository.Update(winningBid);
            }

            return winningBid;
        }
    }
}
