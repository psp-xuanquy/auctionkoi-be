using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Auction.SealedBidAuction.End;
public class EndFixedPriceAuctionCommand : IRequest<Unit>
{
    public string KoiId { get; set; }

    public EndFixedPriceAuctionCommand(string koiId)
    {
        KoiId = koiId;
    }
}
