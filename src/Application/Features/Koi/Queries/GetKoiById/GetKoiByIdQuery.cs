using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Koi.Queries.GetKoiById;
public class GetKoiByIdQuery : IRequest<KoiResponse>, IQuery
{
    public string Id;

    public GetKoiByIdQuery(string id)
    {
        Id = id;
    }
}
