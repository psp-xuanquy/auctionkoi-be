using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.User.Manager.Queries.GetAllCurrentUsers;
public class GetAllCurrentUsersQuery : IRequest<List<GetAllCurrentUsersResponse>>, IQuery
{
    public GetAllCurrentUsersQuery()
    {

    }
}
