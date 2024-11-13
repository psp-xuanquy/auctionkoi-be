using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Bid;
public class PlaceBidCommand : IRequest<string>
{
    public string KoiId { get; }
    public decimal BidAmount { get; set; }

    public PlaceBidCommand(string koiId, decimal bidAmount)
    {
        KoiId = koiId ?? throw new ArgumentNullException(nameof(koiId), "KoiId cannot be null.");
        BidAmount = bidAmount;
    }
}
