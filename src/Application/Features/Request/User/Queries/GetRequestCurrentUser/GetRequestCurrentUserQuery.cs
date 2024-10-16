using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.User.Queries;
using MediatR;

namespace Application.Features.Request.User.Queries.GetRequestCurrentUser;
public class GetRequestCurrentUserQuery : IRequest<List<GetRequestCurrentUserResponse>>, IQuery
{
    public GetRequestCurrentUserQuery()
    {

    }
}

