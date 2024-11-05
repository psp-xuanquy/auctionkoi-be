using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Notification.Queries.Get;
public class GetNotificationQuery : IRequest<List<GetNotificationResponse>>, IQuery
{
    public GetNotificationQuery()
    {

    }
}

