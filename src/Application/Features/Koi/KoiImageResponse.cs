using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.Koi;
public class KoiImageResponse : IMapFrom<KoiImageEntity>
{
    public string? Url { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiImageEntity, KoiImageDto>();
    }
}
