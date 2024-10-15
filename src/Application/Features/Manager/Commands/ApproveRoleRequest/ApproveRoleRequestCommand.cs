using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Application.User.Commands.RegisterKoiBreeder;
using MediatR;

namespace Application.Features.Manager.Commands.ApproveRoleRequest;
public class ApproveRoleRequestCommand : IRequest<string>    
{
    public string KoiBreederID { get; set; }

    public ApproveRoleRequestCommand(string id)
    {
        KoiBreederID = id; 
    }
}
