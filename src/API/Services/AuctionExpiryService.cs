using Domain.Enums;
using Domain.IRepositories.IBaseRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using KoiAuction.Domain.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class AuctionExpiryService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AuctionExpiryService> _logger;
        private readonly IEmailService _emailService;
        private static readonly Random _random = new Random();

        public AuctionExpiryService(IServiceScopeFactory serviceScopeFactory, IEmailService emailService, ILogger<AuctionExpiryService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _emailService = emailService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AuctionExpiryService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var repositories = scope.ServiceProvider.GetRequiredService<IRepositoryFactory>();

                    try
                    {
                        var activeAuctions = await repositories.KoiRepository.GetActiveAuctions(stoppingToken);
                        foreach (var koi in activeAuctions)
                        {
                            if (koi.IsAuctionExpired())
                            {
                                await HandleAuctionExpiry(koi, repositories, stoppingToken);
                            }

                            if (koi.AuctionMethod != null && koi.AuctionMethod.Name == "Descending Bid Auction")
                            {
                                await HandleDescendingAuction(koi, repositories, stoppingToken);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while processing auctions.");
                    }
                }

                _logger.LogInformation("AuctionExpiryService completed a cycle at {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

            _logger.LogInformation("AuctionExpiryService is stopping.");
        }

        private async Task HandleAuctionExpiry(KoiEntity koi, IRepositoryFactory repositories, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling auction expiry for koi: {koiId}", koi.ID);

            var bids = await repositories.BidRepository.GetBidsForKoi(koi.ID, cancellationToken);
            if (bids.Any())
            {
                BidEntity? winningBid;
                try
                {
                    winningBid = koi.AuctionMethod.Name switch
                    {
                        "Fixed Price Sale" => MarkWinningBid(bids, repositories.BidRepository),
                        "Sealed Bid Auction" => bids.OrderByDescending(b => b.BidAmount).First(),
                        "Ascending Bid Auction" => bids.FirstOrDefault(b => b.IsWinningBid == true),
                        _ => throw new Exception("Unknown auction method.")
                    };

                    await _emailService.SendWinningEmail(winningBid.Bidder.Email, koi.Name, winningBid.BidAmount);

                    var transaction = new TransactionEntity
                    {
                        ID = Guid.NewGuid().ToString(),
                        TransactionType = TransactionType.Bought,
                        Status = PaymentStatus.Success,
                        TotalAmount = winningBid.BidAmount,
                        TransactionDate = DateTime.UtcNow,
                        PaymentMethod = "Banking",
                        CommissionRate = 5,
                        BidID = winningBid.ID,
                        KoiID = koi.ID
                    };

                    repositories.TransactionRepository.Add(transaction);
                    await repositories.TransactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while handling auction expiry for koi: {koiId}", koi.ID);
                }
            }
            else
            {
                await RefundBidders(bids, repositories.UserRepository, cancellationToken);
            }

            koi.EndAuction();
            repositories.KoiRepository.Update(koi);
            await repositories.KoiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        
        }

        public async Task RefundBidders(IEnumerable<BidEntity> bids, IUserRepository userRepository, CancellationToken cancellationToken)
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

        private BidEntity MarkWinningBid(IEnumerable<BidEntity> bids, IBidRepository bidRepository)
        {
            if (bids == null || !bids.Any()) return null;

            var winningBid = bids.Count() > 1
                ? bids.ElementAt(new Random().Next(bids.Count()))
                : bids.First();

            winningBid.MarkAsWinningBid();
            bidRepository.Update(winningBid);

            return winningBid;
        }

        private async Task HandleDescendingAuction(KoiEntity koi, IRepositoryFactory repositories, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling descending auction for koi: {koiId}", koi.ID);

            if (koi.AuctionStatus == Domain.Enums.AuctionStatus.OnGoing)
            {
                if (koi.CurrentDescendedPrice != null || koi.CurrentDescendedPrice != 0)
                {
                    if (koi.LowestDescendedPrice <= koi.CurrentDescendedPrice * (100 - koi.DescendingRate) / 100)
                    {
                        koi.CurrentDescendedPrice = koi.CurrentDescendedPrice * (100 - koi.DescendingRate) / 100;
                    }
                    else
                    {
                        _logger.LogInformation("Cannot decrease more for this koi: {koiId}", koi.ID);
                    }
                }
                else
                {
                    koi.CurrentDescendedPrice = koi.InitialPrice * (100 - koi.DescendingRate) / 100;
                }

                repositories.KoiRepository.Update(koi);
                await repositories.KoiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
