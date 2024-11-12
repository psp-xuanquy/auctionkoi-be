using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.Bid.Queries.GetUserPastAuctions;
public class GetUserPastAuctionResponse : IMapFrom<BidEntity>
{
    public string AuctionName { get; set; }
    public decimal BidAmount { get; set; }
    public string AuctionStatus { get; set; }
    public DateTime BidTime { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<BidEntity, GetUserPastAuctionResponse>()
            .ForMember(dest => dest.AuctionName, opt => opt.MapFrom(src => src.Koi.Name))
            .ForMember(dest => dest.AuctionStatus, opt => opt.MapFrom(src => src.Koi.AuctionStatus.ToString()))
            .ForMember(dest => dest.BidAmount, opt => opt.MapFrom(src => src.BidAmount))
            .ForMember(dest => dest.BidTime, opt => opt.MapFrom(src => src.BidTime ?? DateTime.MinValue));
    }
}
