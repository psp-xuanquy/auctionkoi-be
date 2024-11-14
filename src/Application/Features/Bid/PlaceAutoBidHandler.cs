using Application.Features.Bid.Commands.AscendingBidAuction;
using Application.Features.Bid;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Domain.Enums;

public class PlaceAutoBidHandler : IRequestHandler<PlaceAutoBidCommand, string>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IAutoBidRepository _autoBidRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly AscendingBidAuctionHandler _ascendingBidAuctionHandler;

    public PlaceAutoBidHandler(
        IKoiRepository koiRepository,
        IAutoBidRepository autoBidRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        AscendingBidAuctionHandler ascendingBidAuctionHandler)
    {
        _koiRepository = koiRepository;
        _autoBidRepository = autoBidRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _ascendingBidAuctionHandler = ascendingBidAuctionHandler;
    }

    public async Task<string> Handle(PlaceAutoBidCommand command, CancellationToken cancellationToken)
    {
        var bidder = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);

        if (bidder == null)
            throw new NotFoundException("Bidder not found. Please login again.");

        var koi = await _koiRepository.FindAsync(k => k.ID == command.KoiId
            && k.AuctionStatus == AuctionStatus.OnGoing
            && k.StartTime <= DateTime.Now
            && k.EndTime >= DateTime.Now
            && k.AllowAutoBid == true
            , cancellationToken);

        if (koi == null)
            throw new ArgumentException($"Koi with ID {command.KoiId} not found or not allow to use AutoBid.");

        if (koi.AuctionMethod.Name != "Ascending Bid Auction")
            throw new InvalidOperationException("Auto-bid can only be set up for Koi in an Ascending Bid Auction.");

        var existingAutoBid = await _autoBidRepository.FindAsync(ab => ab.BidderID == bidder.Id && ab.KoiID == command.KoiId, cancellationToken);

        if (existingAutoBid != null)
        {
            existingAutoBid.MaxBid = command.MaxBid;
            existingAutoBid.IncrementAmount = command.IncrementAmount;
            _autoBidRepository.Update(existingAutoBid);
        }
        else
        {
            var autoBid = new AutoBidEntity
            {
                KoiID = command.KoiId,
                BidderID = bidder.Id,
                BidTime = DateTime.Now,
                MaxBid = command.MaxBid,
                IncrementAmount = command.IncrementAmount
            };
            _autoBidRepository.Add(autoBid);
        }

        await _autoBidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        await _ascendingBidAuctionHandler.ProcessAutoBids(
            new BidEntity(koi.ID, koi.InitialPrice, bidder.Id, bidder.Balance, koi.InitialPrice),
            koi,
            cancellationToken);

        return "Auto-bid setup successfully.";
    }
}
