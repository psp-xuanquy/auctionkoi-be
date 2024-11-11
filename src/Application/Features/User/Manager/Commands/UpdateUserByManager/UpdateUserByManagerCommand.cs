using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.User.Manager.Commands.UpdateUserByManager;
public class UpdateUserByManagerCommand : IRequest<UserResponse>, IMapFrom<UserEntity>
{
    //public string? Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    //public string FullName { get; set; }
    //public string Address { get; set; }
    //public string Gender { get; set; }
    //public string UrlAvatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateUserByManagerCommand, UserEntity>();
        profile.CreateMap<UserEntity, UserResponse>();
    }
}
