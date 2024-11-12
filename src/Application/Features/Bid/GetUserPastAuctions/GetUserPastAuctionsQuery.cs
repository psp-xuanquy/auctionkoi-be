using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Bid.GetUserPastAuctions;
public class GetUserPastAuctionsQuery : IRequest<List<KoiResponse>>, IQuery
{
    public string UserId { get; set; }

    public GetUserPastAuctionsQuery(string userId)
    {
        UserId = userId;
    }
}
