using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.User.CurrentUser.Commands.UpdateInfo;
public class UpdateCurrentUserInfoCommand : IRequest<UserResponse>, IMapFrom<UserEntity>
{
    //public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    //public string UrlAvatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCurrentUserInfoCommand, UserEntity>();
        profile.CreateMap<UserEntity, UserResponse>();
    }
}
