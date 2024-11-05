using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.Request.KoiBreeder.Commands.ResendRequestKoiAuction;
public class ResendRequestKoiAuctionCommand : IRequest<string>, IMapFrom<KoiEntity>
{
    public string? KoiID { get; set; }
    public string? Name { get; set; }
    public string? Sex { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }
    public string Variety { get; set; }
    public decimal InitialPrice { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? StartTime { get; set; }
    public string? AuctionMethodID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ResendRequestKoiAuctionCommand, KoiEntity>();
    }
}
