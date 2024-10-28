using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using MediatR;

namespace Application.Features.Koi.Commands.Update;
public class UpdateKoiCommand : IRequest<string>, IMapFrom<KoiEntity>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public Sex Sex { get; set; }
    public double Size { get; set; }
    public int Age { get; set; }
    public string Location { get; set; }
    public Variety Variety { get; set; }
    public decimal InitialPrice { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? RequestResponse { get; set; }
    public AuctionRequestStatus AuctionRequestStatus { get; set; }
    public AuctionStatus AuctionStatus { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool AllowAutoBid { get; set; }
    public string? AuctionMethodID { get; set; }
    public string? BreederID { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateKoiCommand, KoiEntity>();
    }
}
