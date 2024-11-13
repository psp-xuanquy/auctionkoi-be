using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Request.Manager.Queries.GetAllKoisRequest;
public class GetAllKoisRequestQuery : IRequest<List<GetAllKoisRequestResponse>>, IQuery
{
    public GetAllKoisRequestQuery()
    {

    }
}
