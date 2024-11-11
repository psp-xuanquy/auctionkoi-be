using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.User.Manager.Commands.DeleteUserByManager;
public class DeleteUserByManagerCommand : IRequest<string>
{
    public string UserId { get; set; }

    public DeleteUserByManagerCommand(string userId)
    {
        UserId = userId;
    }
}

