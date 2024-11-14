using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;

namespace Application.Features.Koi.Queries.GetBidderByKoiId;
public class GetBidderByKoiIdResponse
{
    public List<BidderDto> Bidders { get; set; } = new List<BidderDto>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiEntity, GetBidderByKoiIdResponse>()
        .ForMember(dest => dest.Bidders, opt => opt.MapFrom(src => src.Bids.Select(bid => new BidderDto
        {
            BidderName = bid.Bidder.UserName,
            BidAmount = bid.BidAmount,
            BidTime = bid.BidTime.GetValueOrDefault()
        }).ToList()));
    }
}
