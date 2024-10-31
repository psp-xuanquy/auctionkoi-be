using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Commands.Create;
using AutoMapper;
using KoiAuction.Domain.Entities;

namespace Application.Features.AuctionMethod;

public class AuctionMethodResponse
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuctionMethodResponse, AuctionMethodEntity>();
    }
}
