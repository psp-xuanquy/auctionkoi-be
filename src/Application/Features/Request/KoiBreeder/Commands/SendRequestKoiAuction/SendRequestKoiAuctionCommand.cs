using System.ComponentModel;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using MediatR;

namespace Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
public class SendRequestKoiAuctionCommand : IRequest<string>, IMapFrom<KoiEntity>
{
    public string? Name { get; set; }
    public string? Sex { get; set; }
    public double Size { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }
    public string Variety { get; set; }
    public decimal InitialPrice { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    [DefaultValue(false)]
    public bool? AllowAutoBid { get; set; } 
    public DateTimeOffset StartTime { get; set; }
    public string? AuctionMethodID { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<SendRequestKoiAuctionCommand, KoiEntity>();
    }
}
