using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Request.Manager.Queries.GetPendingKoiRequestById;
public class GetKoiRequestByIdQuery : IRequest<GetKoiRequestByIdResponse>, IQuery
{
    public string KoiId { get; set; }
    public GetKoiRequestByIdQuery(string koiId)
    {
        KoiId = koiId;
    }
}
