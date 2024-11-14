using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Bid;
public class PlaceAutoBidCommand : IRequest<string>
{
    public string KoiId { get; set; }
    public decimal MaxBid { get; set; }
    public decimal IncrementAmount { get; set; }

    public PlaceAutoBidCommand(string koiId, decimal maxBid, decimal incrementAmount)
    {
        KoiId = koiId;
        MaxBid = maxBid;
        IncrementAmount = incrementAmount;
    }
}
