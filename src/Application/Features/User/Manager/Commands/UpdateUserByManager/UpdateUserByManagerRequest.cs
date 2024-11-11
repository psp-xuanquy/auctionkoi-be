using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.User;
using MediatR;

namespace Application.Features.User.Manager.Commands.UpdateUserByManager;
public class UpdateUserByManagerRequest : IRequest<UserResponse>
{
    public string Id { get; set; }
    public UpdateUserByManagerCommand Command { get; set; }

    public UpdateUserByManagerRequest(string id, UpdateUserByManagerCommand command)
    {
        Id = id;
        Command = command;
    }
}
