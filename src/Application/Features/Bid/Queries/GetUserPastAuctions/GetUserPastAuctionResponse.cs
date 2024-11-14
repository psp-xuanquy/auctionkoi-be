using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;

namespace Application.Features.Bid.Queries.GetUserPastAuctions;
public class GetUserPastAuctionResponse
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public Sex Sex { get; set; }
    public double Size { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }
    public Variety Variety { get; set; }
    public decimal ReservePrice { get; set; }
    public decimal HighestPrice { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public AuctionRequestStatus AuctionRequestStatus { get; set; }
    public AuctionStatus AuctionStatus { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool AllowAutoBid { get; set; }
    public string? AuctionMethodName { get; set; }
    public string? BreederName { get; set; }
    public string? Contact { get; set; }
    public List<BidderDto> Bidders { get; set; } = new List<BidderDto>();
    public List<KoiImageDto> KoiImages { get; set; } = new List<KoiImageDto>();
    public DeliveryStatus DeliveryStatus { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiEntity, KoiResponse>()
        .ForMember(dest => dest.ReservePrice, opt => opt.MapFrom(src => src.InitialPrice))
        .ForMember(dest => dest.HighestPrice, opt => opt.MapFrom(src => src.Bids.Max(bid => bid.BidAmount)))
        .ForMember(dest => dest.AuctionMethodName, opt => opt.MapFrom(src => src.AuctionMethod.Name))
        .ForMember(dest => dest.BreederName, opt => opt.MapFrom(src => src.Breeder.UserName))
        .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Breeder.PhoneNumber))
        .ForMember(dest => dest.Bidders, opt => opt.MapFrom(src => src.Bids.Select(bid => new BidderDto
        {
            BidderName = bid.Bidder.UserName,
            BidAmount = bid.BidAmount,
            BidTime = bid.BidTime.GetValueOrDefault()
        }).ToList()))
        .ForMember(dest => dest.KoiImages, opt => opt.MapFrom(src => src.KoiImages.Select(img => new KoiImageDto
        {
            Url = img.Url,
            KoiName = img.Koi.Name,
        }).ToList()))
        .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => src.AuctionStatus == AuctionStatus.Ended ? DeliveryStatus.OnGoing : DeliveryStatus.Finished));
    }

    //public string AuctionName { get; set; }
    //public decimal BidAmount { get; set; }
    //public string AuctionStatus { get; set; }
    //public DateTime BidTime { get; set; }

    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<BidEntity, GetUserPastAuctionResponse>()
    //        .ForMember(dest => dest.AuctionName, opt => opt.MapFrom(src => src.Koi.Name))
    //        .ForMember(dest => dest.AuctionStatus, opt => opt.MapFrom(src => src.Koi.AuctionStatus.ToString()))
    //        .ForMember(dest => dest.BidAmount, opt => opt.MapFrom(src => src.BidAmount))
    //        .ForMember(dest => dest.BidTime, opt => opt.MapFrom(src => src.BidTime ?? DateTime.MinValue));
    //}
}
