using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi.Commands.Create;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;

namespace Application.Features.Koi;

public class KoiResponse 
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public Sex Sex { get; set; }
    public double Size { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }
    public Variety Variety { get; set; }
    public decimal ReservePrice { get; set; }
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

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiEntity, KoiResponse>()
        .ForMember(dest => dest.ReservePrice, opt => opt.MapFrom(src => src.InitialPrice))
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
        }).ToList()));
    }
}
