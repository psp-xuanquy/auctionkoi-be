using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.User.CurrentUser.Commands.UpdateInfo;
public class UpdateCurrentUserInfoRequest : IRequest<UserResponse>
{
    public string Id { get; set; }
    public UpdateCurrentUserInfoCommand Command { get; set; }

    public UpdateCurrentUserInfoRequest(string id, UpdateCurrentUserInfoCommand command)
    {
        Id = id;
        Command = command;
    }
}
