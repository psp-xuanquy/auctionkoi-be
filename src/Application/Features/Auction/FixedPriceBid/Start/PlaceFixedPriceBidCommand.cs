using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Auction.FixedPriceBid.Start;
public class PlaceFixedPriceBidCommand : IRequest<Unit>
{
    public string KoiId { get; }
    public string BidderId { get; }

    public PlaceFixedPriceBidCommand(string koiId, string bidderId)
    {
        KoiId = koiId;
        BidderId = bidderId;
    }
}
