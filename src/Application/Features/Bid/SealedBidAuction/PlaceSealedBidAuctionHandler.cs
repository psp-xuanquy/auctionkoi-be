using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public PlaceSealedBidAuctionHandler(
            ICurrentUserService currentUserService,
            IKoiRepository koiRepository,
            IBidRepository bidRepository,
            IUserRepository userRepository,
            IEmailService emailService)
            : base(koiRepository, bidRepository, userRepository, currentUserService)
        {
            _emailService = emailService;
        }

        public async Task<string> Handle(PlaceSealedBidAuctionCommand request, CancellationToken cancellationToken)
        {
            var bidder = await GetCurrentBidder(cancellationToken);
            var koi = await GetKoiForAuction(request.KoiId, "Sealed Bid Auction", cancellationToken);

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

            if (koi.IsAuctionExpired())
            {
                await HandleAuctionExpiry(koi, cancellationToken);
                throw new InvalidOperationException("The auction has already expired.");
            }

            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value.ToString();
        }

        private async Task HandleAuctionExpiry(KoiEntity koi, CancellationToken cancellationToken)
        {
            koi.EndAuction();
            _koiRepository.Update(koi);

            var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
            if (bids.Any())
            {
                var winningBid = bids.OrderByDescending(b => b.BidAmount).First();
                await _emailService.SendWinningEmail(winningBid.BidderID, koi.Name, winningBid.BidAmount);
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

            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateBid(decimal bidAmount, string koiId, string bidderId, CancellationToken cancellationToken)
        {
            var koi = await _koiRepository.FindAsync(k => k.ID == koiId, cancellationToken);
            if (koi == null)
                throw new NotFoundException($"Koi with ID '{koiId}' not found.");

            var bidder = await _userRepository.FindAsync(b => b.Id == bidderId, cancellationToken);
            if (bidder == null)
                throw new NotFoundException("Bidder not found.");

            if (bidAmount < koi.InitialPrice)
                throw new Exception($"Bid amount must be greater than the starting price of {koi.InitialPrice:C}.");

            if (bidAmount > bidder.Balance)
                throw new Exception("Bid amount exceeds available balance.");

            var existingBid = await _bidRepository.GetUserBidForKoi(koiId, bidderId, cancellationToken);
            if (existingBid != null)
                throw new Exception("You have already placed a bid for this auction.");
        }
    }
}
