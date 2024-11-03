using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Bid.DescendingBidAuction;
public class DescendingBidAuctionCommand : IRequest<Unit>
{
    public string KoiId { get; }
    public decimal BidAmount { get; }

    public DescendingBidAuctionCommand(string koiId, decimal bidAmount)
    {
        KoiId = koiId;
        BidAmount = bidAmount;
    }
}
