using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Application.User.Queries;
using KoiAuction.Domain.Entities;

namespace Application.Features.Request.User.Queries.GetRequestCurrentUser;
public class GetRequestCurrentUserResponse : IMapFrom<KoiBreederEntity>
{
    public string? KoiBreederID { get; set; }
    public string? KoiFarmName { get; set; }
    public string? KoiFarmDescription { get; set; }
    public string? KoiFarmLocation { get; set; }
    public string? KoiFarmImage { get; set; }
    public string? RequestResponse { get; set; }
    public RoleRequestStatus RoleRequestStatus { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiBreederEntity, GetRequestCurrentUserResponse>()
            .ForMember(dest => dest.KoiBreederID, opt => opt.MapFrom(src => src.ID));
    }
}
