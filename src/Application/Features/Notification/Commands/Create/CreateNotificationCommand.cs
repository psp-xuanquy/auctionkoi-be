using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
using AutoMapper;
using Domain.Entities;
using KoiAuction.Application.Common.Mappings;
using MediatR;

namespace Application.Features.Notification.Commands.Create;
public class CreateNotificationCommand : IRequest<string>, IMapFrom<NotificationEntity>
{
    public string Message { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateNotificationCommand, NotificationEntity>();
    }
}
