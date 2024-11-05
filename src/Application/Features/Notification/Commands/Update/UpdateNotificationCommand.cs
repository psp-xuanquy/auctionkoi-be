using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Notification.Queries.Get;
using MediatR;

namespace Application.Features.Notification.Commands.Update;
public class UpdateNotificationCommand : IRequest<GetNotificationResponse>
{
    public string Id { get; set; }

    public UpdateNotificationCommand(string id)
    {
        Id = id;
    }
}
