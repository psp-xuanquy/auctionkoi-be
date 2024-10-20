using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Auction.SealedBidAuction.Start;
public class PlaceSealedBidAuctionHandler : IRequestHandler<PlaceSealedBidAuctionCommand, Unit>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IUserRepository _userRepository;

    public PlaceSealedBidAuctionHandler(IKoiRepository koiRepository, IBidRepository bidRepository, IUserRepository userRepository)
    {
        _koiRepository = koiRepository;
        _bidRepository = bidRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(PlaceSealedBidAuctionCommand request, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(k => k.ID == request.KoiId && k.DeletedTime == null, cancellationToken);
        if (koi == null || koi.AuctionStatus != AuctionStatus.OnGoing)
            throw new InvalidOperationException("Auction for this Koi is not active.");

        var bidder = await _userRepository.FindAsync(b => b.Id == request.BidderId && b.DeletedTime == null, cancellationToken);
        if (bidder == null)
            throw new InvalidOperationException("Bidder does not exist.");

        if (request.BidAmount <= koi.InitialPrice || request.BidAmount > bidder.Balance)
            throw new InvalidOperationException("Invalid bid amount.");

        var existingBid = await _bidRepository.GetUserBidForKoi(request.BidderId, request.KoiId);
        if (existingBid != null)
            throw new InvalidOperationException("You have already placed a bid.");

        var bid = new BidEntity(koi.ID, request.BidAmount, bidder.Id);
        _bidRepository.Add(bid);

        await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
