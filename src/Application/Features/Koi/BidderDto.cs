using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.Koi;
public class BidderDto : IMapFrom<BidEntity>
{
    public string BidderName { get; set; }
    public decimal BidAmount { get; set; }
    public DateTime? BidTime { get; set; }
    //public string KoiID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<BidEntity, BidderDto>()
            .ForMember(dest => dest.BidderName, opt => opt.MapFrom(src => src.Bidder.UserName));
    }
}
