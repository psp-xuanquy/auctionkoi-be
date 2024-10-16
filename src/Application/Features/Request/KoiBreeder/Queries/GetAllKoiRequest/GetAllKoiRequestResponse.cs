using Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
public class GetAllKoiRequestResponse : IMapFrom<KoiEntity>
{
    public string? ID { get; set; }
    public string? Name { get; set; }
    public string? Sex { get; set; }
    public double Size { get; set; }
    public int Age { get; set; }
    public decimal InitialPrice { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool? AllowAutoBid { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? RequestResponse { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public Variety Variety { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public AuctionRequestStatus AuctionRequestStatus { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public AuctionStatus AuctionStatus { get; set; }
    public string? AuctionMethod { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiEntity, GetAllKoiRequestResponse>()
            .ForMember(dest => dest.AuctionMethod, opt => opt.MapFrom(src => src.AuctionMethod.Name))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToString("dd-MM-yyyy HH:mm")))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToString("dd-MM-yyyy HH:mm"))); 
    }

}
