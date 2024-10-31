using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.User.CurrentUser.Queries.GetCurrentUser;
public class GetCurrentUserQuery : IRequest<GetCurrentUserResponse>, IQuery
{
    public string UserId { get; }

    public GetCurrentUserQuery(string userId)
    {
        UserId = userId;
    }
}
