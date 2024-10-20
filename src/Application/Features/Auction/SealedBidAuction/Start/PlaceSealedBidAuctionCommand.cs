using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Auction.SealedBidAuction.Start;
public class PlaceSealedBidAuctionCommand : IRequest<Unit>
{
    public string KoiId { get; }
    public string BidderId { get; }
    public decimal BidAmount { get; set; }

    public PlaceSealedBidAuctionCommand(string koiId, string bidderId, decimal bidAmount)
    {
        KoiId = koiId;
        BidderId = bidderId;
        BidAmount = bidAmount;
    }
}
