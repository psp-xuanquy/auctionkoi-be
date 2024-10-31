using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.User.CurrentUser.Queries.GetCurrentUser;
public class GetCurrentUserResponse : IMapFrom<UserEntity>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserEntity, GetCurrentUserResponse>();

    }
}
