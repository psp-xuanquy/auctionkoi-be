using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi.Queries.GetBidderByKoiId;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Koi.Queries.GetCurrentBidderByKoiId;
public class GetCurrentBidderByKoiIdQuery : IRequest<BidderDto>, IQuery
{
    public string KoiId { get; set; }
    public GetCurrentBidderByKoiIdQuery(string koiId)
    {
        KoiId = koiId;
    }
}
