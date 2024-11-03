using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Koi.Queries.GetAllActiveAuctions;
public class GetAllActiveAuctionsQuery : IRequest<List<KoiResponse>>, IQuery
{
    public GetAllActiveAuctionsQuery()
    {

    }
}
