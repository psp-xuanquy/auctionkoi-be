using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.Koi;
public class KoiImageDto : IMapFrom<KoiImageEntity>
{
    public string? Url { get; set; }
    public string? KoiName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiImageEntity, KoiImageDto>();
    }
}
