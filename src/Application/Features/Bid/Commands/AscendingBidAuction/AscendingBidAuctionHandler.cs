using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Bid.Commands.AscendingBidAuction
{
    public class AscendingBidAuctionHandler : BaseAuctionHandler, IRequestHandler<AscendingBidAuctionCommand, string>
    {
        private readonly IEmailService _emailService;

        public AscendingBidAuctionHandler(
            IKoiRepository koiRepository,
            IBidRepository bidRepository,
            IAutoBidRepository autoBidRepository,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IEmailService emailService)
            : base(koiRepository, bidRepository, autoBidRepository, userRepository, currentUserService)
        {
            _emailService = emailService;
        }

        public async Task<string> Handle(AscendingBidAuctionCommand request, CancellationToken cancellationToken)
        {
            var bidder = await GetCurrentBidder(cancellationToken);
            var koi = await GetKoiForAuction(request.KoiId, "Ascending Bid Auction", cancellationToken);

            await ValidateBid(request, bidder, koi, cancellationToken);

            if (koi.IsAuctionExpired())
            {
                await HandleAuctionExpiry(koi, cancellationToken);
                throw new InvalidOperationException("The auction has already expired.");
            }

            await ResetWinningBids(koi, cancellationToken);

            var temporaryBidAmount = request.BidAmount;
            bidder.Balance -= temporaryBidAmount;
            _userRepository.Update(bidder);

            var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice)
            {
                IsWinningBid = true
            };

            _bidRepository.Add(bid);

            if (koi.AuctionStatus == AuctionStatus.NotStarted || koi.AuctionStatus == AuctionStatus.OnGoing)
            {
                koi.StartAuction();
                _koiRepository.Update(koi);
            }

            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await ProcessAutoBids(bid, koi, cancellationToken);
            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            if (bidder.Balance < 0)
            {
                bidder.Balance += temporaryBidAmount;
                _userRepository.Update(bidder);
                await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            return $"You have successfully bid {request.BidAmount} for the fish {koi.Name}.";
        }

        private async Task ValidateBid(AscendingBidAuctionCommand request, UserEntity bidder, KoiEntity koi, CancellationToken cancellationToken)
        {
            if (request.BidAmount > bidder.Balance)
                throw new InvalidOperationException("Bid amount cannot exceed the user's balance.");

            await ValidateBidAmount(request.BidAmount, koi.ID, bidder.Id, cancellationToken);

            var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);

            var winningBid = bids.FirstOrDefault(b => b.IsWinningBid);
            if (winningBid != null)
            {
                if (request.BidAmount <= winningBid.BidAmount)
                {
                    throw new InvalidOperationException("Bid amount must be higher than the current winning bid.");
                }
            }
            else
            {
                if (request.BidAmount <= koi.InitialPrice)
                {
                    throw new InvalidOperationException("Bid amount must be higher than the initial price.");
                }
            }
        }

        private async Task ResetWinningBids(KoiEntity koi, CancellationToken cancellationToken)
        {
            var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
            foreach (var bid in bids)
            {
                bid.IsWinningBid = false;
                _bidRepository.Update(bid);
            }
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

        private BidEntity MarkWinningBid(IEnumerable<BidEntity> bids)
        {
            var winningBid = bids.OrderByDescending(b => b.BidAmount).ThenByDescending(b => b.CreatedTime).FirstOrDefault();
            if (winningBid != null)
            {
                winningBid.MarkAsWinningBid();
                _bidRepository.Update(winningBid);
            }

            return winningBid;
        }

        public async Task ProcessAutoBids(BidEntity currentBid, KoiEntity koi, CancellationToken cancellationToken)
        {
            var autoBids = await _autoBidRepository.GetAutoBidsForKoi(koi.ID, cancellationToken);
            foreach (var autoBid in autoBids)
            {
                if (autoBid.BidderID == currentBid.BidderID) continue;

                var nextBidAmount = currentBid.BidAmount + autoBid.IncrementAmount;
                if (nextBidAmount <= autoBid.MaxBid && nextBidAmount <= autoBid.Bidder?.Balance)
                {
                    var newBid = new BidEntity(koi.ID, nextBidAmount, autoBid.BidderID, autoBid.Bidder?.Balance ?? 0, koi.InitialPrice)
                    {
                        IsAutoBid = true,
                        IsWinningBid = true
                    };

                    await ResetWinningBids(koi, cancellationToken);
                    _bidRepository.Add(newBid);

                    await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                    await ProcessAutoBids(newBid, koi, cancellationToken);
                }
            }
        }
    }
}
