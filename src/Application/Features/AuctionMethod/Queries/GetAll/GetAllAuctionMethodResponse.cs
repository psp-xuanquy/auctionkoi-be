using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.AuctionMethod.Queries.GetAll;
public class GetAllAuctionMethodResponse : IMapFrom<AuctionMethodEntity>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuctionMethodEntity, GetAllAuctionMethodResponse>();
            
    }
}
