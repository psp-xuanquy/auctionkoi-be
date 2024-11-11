using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.User.CurrentUser.Commands.UpdateInfo;
using MediatR;

namespace Application.Features.User.CurrentUser.Commands.UpdateAvatar;
public class UpdateCurrentUserAvatarRequest : IRequest<UserResponse>
{
    public string Id { get; set; }
    public UpdateCurrentUserAvatarCommand Command { get; set; }

    public UpdateCurrentUserAvatarRequest(string id, UpdateCurrentUserAvatarCommand command)
    {
        Id = id;
        Command = command;
    }
}
