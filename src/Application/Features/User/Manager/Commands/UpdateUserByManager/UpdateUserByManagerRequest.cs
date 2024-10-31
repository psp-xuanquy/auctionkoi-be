﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.User;
using Application.Features.User.Manager.Commands.Update;
using MediatR;

namespace Application.Features.AuctionMethod.Commands.Update;
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
