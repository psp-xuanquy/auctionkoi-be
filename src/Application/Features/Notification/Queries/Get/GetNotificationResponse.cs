using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using AutoMapper;
using Domain.Entities;
using KoiAuction.Application.Common.Mappings;

namespace Application.Features.Notification.Queries.Get;
public class GetNotificationResponse : IMapFrom<NotificationEntity>
{
    public string Id { get; set; }
    public string Message { get; set; }
    public bool MarkAsRead { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<NotificationEntity, GetNotificationResponse>();
    }
}
