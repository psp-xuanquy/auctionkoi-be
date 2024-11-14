using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Bid.Queries.CheckSealedBidAuction;
public class CheckSealedBidAuctionCommand : IRequest<bool>
{
    public string KoiId { get; }

    public CheckSealedBidAuctionCommand(string koiId)
    {
        KoiId = koiId ?? throw new ArgumentNullException(nameof(koiId), "KoiId cannot be null.");
    }
}
