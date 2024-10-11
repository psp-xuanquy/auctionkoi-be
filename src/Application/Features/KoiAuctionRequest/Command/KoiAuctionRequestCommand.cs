using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.KoiAuctionRequest.Command;
public class KoiAuctionRequestCommand : IRequest<string>, IMapFrom<KoiAuctionRequest>
{
    public required string BreederId { get; set; }
    public required string KoiId { get; set; }
    public decimal InitialPrice { get; set; }
    public bool AllowAutoBid { get; set; }
    public required string AuctionMethodId { get; set; }
    public bool IsInspectionRequired { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiAuctionRequestCommand, KoiAuctionRequest>();
    }
}
