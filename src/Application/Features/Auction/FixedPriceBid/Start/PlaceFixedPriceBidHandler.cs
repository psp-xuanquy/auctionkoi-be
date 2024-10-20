using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Auction.FixedPriceBid.Start;
public class PlaceFixedPriceBidHandler : IRequestHandler<PlaceFixedPriceBidCommand, Unit>
{
    private readonly IKoiRepository _koiRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IUserRepository _userRepository;

    public PlaceFixedPriceBidHandler(IKoiRepository koiRepository, IBidRepository bidRepository, IUserRepository userRepository)
    {
        _koiRepository = koiRepository;
        _bidRepository = bidRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(PlaceFixedPriceBidCommand request, CancellationToken cancellationToken)
    {
        var koi = await _koiRepository.FindAsync(k => k.ID == request.KoiId && k.DeletedTime == null, cancellationToken);
        if (koi == null || koi.AuctionStatus != AuctionStatus.OnGoing)
            throw new InvalidOperationException("Auction for this Koi is not active.");

        var bidder = await _userRepository.FindAsync(b => b.Id == request.BidderId && b.DeletedTime == null, cancellationToken);
        if (bidder == null)
            throw new InvalidOperationException("Bidder does not exist.");

        if (bidder.Balance < koi.InitialPrice)
            throw new InvalidOperationException("Insufficient balance.");

        var bid = new BidEntity(koi.ID, koi.InitialPrice, bidder.Id);
        _bidRepository.Add(bid);

        await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
