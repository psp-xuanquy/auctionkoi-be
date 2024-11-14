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

            var bids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
            if (bids.Count() > 0)
            {
                foreach (var b in bids)
                {
                    b.IsWinningBid = false;
                    _bidRepository.Update(b);
                }
            }

            var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id, bidder.Balance, koi.InitialPrice)
            {
                IsWinningBid = true
            };

            _bidRepository.Add(bid);

            if (koi.AuctionStatus == AuctionStatus.NotStarted)
            {
                koi.StartAuction();
                _koiRepository.Update(koi);
            }

            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            await ProcessAutoBids(bid, koi, cancellationToken);

            var winningBid = bids.OrderByDescending(b => b.BidAmount).ThenByDescending(b => b.CreatedTime).FirstOrDefault();
            if (winningBid != null)
            {
                var winner = await _userRepository.FindAsync(w => w.Id == winningBid.BidderID, cancellationToken);
                if (winner != null)
                {
                    winner.Balance -= winningBid.BidAmount;
                    _userRepository.Update(winner);

                    await _emailService.SendWinningEmail(winner.Email, koi.Name, winningBid.BidAmount);
                }
            }

            await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            koi.EndAuction();
            _koiRepository.Update(koi);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return $"You have successfully bid {request.BidAmount} for the fish {koi.Name}.";
        }

        private async Task ValidateBid(AscendingBidAuctionCommand request, UserEntity bidder, KoiEntity koi, CancellationToken cancellationToken)
        {
            if (request.BidAmount > bidder.Balance)
                throw new InvalidOperationException("Bid amount cannot exceed the user's balance.");

            await ValidateBidAmount(request.BidAmount, koi.ID, bidder.Id, cancellationToken);

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
                    if (request.BidAmount <= winningBid.BidAmount)
                    {
                        throw new InvalidOperationException("Your new bid must be higher than your previous winning bid.");
                    }
                }
                else
                {
                    if (request.BidAmount <= winningBid.BidAmount)
                    {
                        throw new InvalidOperationException("Bid amount must be higher than the current winning bid.");
                    }
                }
            }
            else
            {
                if (request.BidAmount <= koi.InitialPrice)
                {
                    throw new InvalidOperationException("Bid amount must be higher than the initial price.");
                }
            }

            if (koi.IsAuctionExpired())
            {
                throw new InvalidOperationException("The auction has already expired.");
            }
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

                     var allBids = await _bidRepository.GetBidsForKoi(koi.ID, cancellationToken);
                    foreach (var b in allBids)
                    {
                        b.IsWinningBid = false;
                        _bidRepository.Update(b);
                    }

                    _bidRepository.Add(newBid);

                    await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                    await ProcessAutoBids(newBid, koi, cancellationToken);
                }
            }
        }
    }
}
