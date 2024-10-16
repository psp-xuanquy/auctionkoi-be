using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using KoiAuction.Application.Common.Mappings;
using MediatR;

namespace Application.Features.Request.Manager.Commands.ApproveRoleRequest;
public class ApproveRoleRequestCommand : IRequest<string>
{
    public string KoiBreederID { get; set; }

}
