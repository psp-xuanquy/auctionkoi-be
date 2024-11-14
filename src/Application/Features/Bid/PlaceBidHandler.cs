using Application.Features.Bid.Commands.AscendingBidAuction;
using Application.Features.Bid.Commands.DescendingBidAuction;
using Application.Features.Bid.Commands.FixedPriceBid;
using Application.Features.Bid.Commands.SealedBidAuction;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Bid;
public class PlaceBidHandler : IRequestHandler<PlaceBidCommand, string>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IAutoBidRepository _autoBidRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailService _emailService;

    public PlaceBidHandler(IKoiRepository koiRepository, IBidRepository bidRepository, IAutoBidRepository autoBidRepository, IUserRepository userRepository, ICurrentUserService currentUserService, IEmailService emailService)
    {
        _koiRepository = koiRepository;
        _bidRepository = bidRepository;
        _autoBidRepository = autoBidRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _emailService = emailService;
    }

    public async Task<string> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(k => k.ID == request.KoiId
            && k.AuctionStatus == AuctionStatus.OnGoing
            && k.StartTime <= DateTime.Now
            && k.EndTime >= DateTime.Now
            , cancellationToken);

        if (koi == null)
            throw new ArgumentException($"Koi with ID {request.KoiId} not found.");

        var auctionMethod = koi.AuctionMethod;

        switch (auctionMethod?.Name)
        {
            case "Fixed Price Sale":
                return await HandleFixedPriceAuction(request, koi, cancellationToken);

            case "Sealed Bid Auction":
                return await HandleSealedBidAuction(request, koi, cancellationToken);

            case "Ascending Bid Auction":
                return await HandleAscendingBidAuction(request, koi, cancellationToken);

            case "Descending Bid Auction":
                return await HandleDescendingBidAuction(request, koi, cancellationToken);


            default:
                throw new InvalidOperationException($"Unknown auction method for koi {koi.Name}.");
        }
    }

    // Handle Fixed Price Auction Logic
    private async Task<string> HandleFixedPriceAuction(PlaceBidCommand request, KoiEntity koi, CancellationToken cancellationToken)
    {
        var handler = new FixedPriceAuctionHandler(_currentUserService, _koiRepository, _bidRepository, _autoBidRepository, _userRepository, null);
        return await handler.Handle(new PlaceFixedPriceBidCommand(request.KoiId, request.BidAmount), cancellationToken);
    }

    // Handle Ascending Bid Auction Logic
    private async Task<string> HandleAscendingBidAuction(PlaceBidCommand request, KoiEntity koi, CancellationToken cancellationToken)
    {
        var handler = new AscendingBidAuctionHandler(_koiRepository, _bidRepository, _autoBidRepository, _userRepository, _currentUserService, _emailService);
        return await handler.Handle(new AscendingBidAuctionCommand(request.KoiId, request.BidAmount), cancellationToken);
    }

    // Handle Descending Bid Auction Logic
    private async Task<string> HandleDescendingBidAuction(PlaceBidCommand request, KoiEntity koi, CancellationToken cancellationToken)
    {
        var handler = new DescendingBidAuctionHandler(_koiRepository, _bidRepository, _autoBidRepository, _userRepository, _currentUserService);
        return await handler.Handle(new DescendingBidAuctionCommand(request.KoiId, request.BidAmount), cancellationToken);
    }

    // Handle Sealed Bid Auction Logic
    private async Task<string> HandleSealedBidAuction(PlaceBidCommand request, KoiEntity koi, CancellationToken cancellationToken)
    {
        var handler = new PlaceSealedBidAuctionHandler(_currentUserService, _koiRepository, _bidRepository, _autoBidRepository, _userRepository, null);
        return await handler.Handle(new PlaceSealedBidAuctionCommand(request.KoiId, request.BidAmount), cancellationToken);
    }
}
