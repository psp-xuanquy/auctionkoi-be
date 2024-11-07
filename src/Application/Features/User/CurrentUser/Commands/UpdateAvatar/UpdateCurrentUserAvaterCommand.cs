using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.User.CurrentUser.Commands.UpdateAvatar;
public class UpdateCurrentUserAvatarCommand : IRequest<UserResponse>, IMapFrom<UserEntity>
{
    public string UrlAvatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCurrentUserAvatarCommand, UserEntity>();
        profile.CreateMap<UserEntity, UserResponse>();
    }
}
