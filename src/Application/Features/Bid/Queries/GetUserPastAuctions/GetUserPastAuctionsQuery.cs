using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Bid.Queries.GetUserPastAuctions;
public class GetUserPastAuctionsQuery : IRequest<List<GetUserPastAuctionResponse>>, IQuery
{
    public string UserId { get; set; }

    public GetUserPastAuctionsQuery(string userId)
    {
        UserId = userId;
    }
}
