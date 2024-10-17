using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using MediatR;

namespace Application.Features.AuctionMethod.Commands.Create;
public class CreateAuctionMethodCommand : IRequest<string>, IMapFrom<AuctionMethodEntity>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateAuctionMethodCommand, AuctionMethodEntity>();
    }
}

