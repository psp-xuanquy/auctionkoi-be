using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Bid.FixedPriceBid;
public class PlaceFixedPriceBidCommand : IRequest<Unit>
{
    public string KoiId { get; }
    public decimal BidAmount { get; }

    public PlaceFixedPriceBidCommand(string koiId, decimal bidAmount)
    {
        KoiId = koiId;
        BidAmount = bidAmount;
    }
}
