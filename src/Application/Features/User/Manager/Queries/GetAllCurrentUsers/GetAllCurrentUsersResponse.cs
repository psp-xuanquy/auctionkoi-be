using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.User.Manager.Queries.GetAllCurrentUsers;
public class GetAllCurrentUsersResponse : IMapFrom<UserEntity>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string? UrlAvatar { get; set; }
    public string Phone { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserEntity, GetAllCurrentUsersResponse>();
    }
}
