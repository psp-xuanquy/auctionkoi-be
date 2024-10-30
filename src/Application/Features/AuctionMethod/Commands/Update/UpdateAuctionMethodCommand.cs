using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.AuctionMethod.Commands.Update;
public class UpdateAuctionMethodCommand : IRequest<AuctionMethodResponse>, IMapFrom<AuctionMethodEntity>
{
    //public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateAuctionMethodCommand, AuctionMethodEntity>();
        profile.CreateMap<AuctionMethodEntity, AuctionMethodResponse>();
    }
}
