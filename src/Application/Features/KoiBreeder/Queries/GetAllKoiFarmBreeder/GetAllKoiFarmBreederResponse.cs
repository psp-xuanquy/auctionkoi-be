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

namespace Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
public class GetAllKoiFarmBreederResponse : IMapFrom<KoiBreederEntity>
{
    //public string? ID { get; set; }
    public string? KoiFarmName { get; set; }
    public string? KoiFarmDescription { get; set; }
    public string? KoiFarmLocation { get; set; }
    public string? KoiFarmImage { get; set; }
    public string Status { get; set; }
    public string? BreederName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiBreederEntity, GetAllKoiFarmBreederResponse>()
            .ForMember(dest => dest.BreederName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.User.Status ? "Active" : "Inactive")); ;
    }

}
