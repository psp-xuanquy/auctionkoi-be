using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Auction.FixedPriceBid.End;
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
        var bids = await _bidRepository.GetBidsForKoi(request.KoiId, cancellationToken);

        // Nếu có hơn một bid, chọn ngẫu nhiên bid thắng
        if (bids.Count() > 1)
        {
            var random = new Random();
            var winningBid = bids.ElementAt(random.Next(bids.Count()));
            winningBid.MarkAsWinningBid();
            _bidRepository.Update(winningBid);
        }
        // Nếu chỉ có một bid, đánh dấu nó là bid thắng
        else if (bids.Count() == 1)
        {
            var bid = bids.First(); // Lấy phần tử đầu tiên
            bid.MarkAsWinningBid();
            _bidRepository.Update(bid);
        }

        var koi = await _koiRepository.FindAsync(k => k.ID == request.KoiId && k.DeletedTime == null, cancellationToken);
        if (koi == null)
            throw new InvalidOperationException("Koi not found.");

        koi.EndAuction();
        _koiRepository.Update(koi);

        return Unit.Value;
    }
}
