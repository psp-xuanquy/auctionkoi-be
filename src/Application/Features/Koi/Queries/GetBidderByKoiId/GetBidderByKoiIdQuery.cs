using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi.Queries.GetBidderByKoiId;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Koi.Queries.GetAllActiveAuctions;
public class GetBidderByKoiIdQuery : IRequest<List<GetBidderByKoiIdResponse>>, IQuery
{
    public string KoiId { get; set; }
    public GetBidderByKoiIdQuery(string koiId)
    {
        KoiId = koiId;
    }
}
