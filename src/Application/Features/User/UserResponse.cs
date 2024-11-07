using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Entities;

namespace Application.Features.User;

public class UserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string UrlAvatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UserResponse, UserEntity>();
    }
}
