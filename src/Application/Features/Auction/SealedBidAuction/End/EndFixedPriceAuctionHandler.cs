using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Auction.SealedBidAuction.End;
public class EndFixedPriceAuctionHandler : IRequestHandler<EndFixedPriceAuctionCommand, Unit>
{
    private readonly IBidRepository _bidRepository;
    private readonly IKoiRepository _koiRepository;

    public EndFixedPriceAuctionHandler(IBidRepository bidRepository, IKoiRepository koiRepository)
    {
        _bidRepository = bidRepository;
        _koiRepository = koiRepository;
    }

    public async Task<Unit> Handle(EndFixedPriceAuctionCommand request, CancellationToken cancellationToken)
    {
        var highestBid = await _bidRepository.GetHighestBidForKoi(request.KoiId, cancellationToken);
        if (highestBid != null)
        {
            highestBid.MarkAsWinningBid();
            _bidRepository.Update(highestBid);
        }

        var koi = await _koiRepository.FindAsync(k => k.ID == request.KoiId && k.DeletedTime == null, cancellationToken);
        if (koi == null)
            throw new InvalidOperationException("Koi not found.");

        koi.EndAuction();
        _koiRepository.Update(koi);

        return Unit.Value;
    }
}
