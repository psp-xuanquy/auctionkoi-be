using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Manager.Queries.GetAllPendingRoles;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
public class GetAllKoiRequestQuery : IRequest<List<GetAllKoiRequestResponse>>, IQuery
{
    public GetAllKoiRequestQuery()
    {

    }
}
