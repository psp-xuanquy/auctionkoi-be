using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.AuctionMethod.Queries.GetAll;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.Koi.Queries.GetAllActiveAuctions;
public class GetAllActiveAuctionsResponse : IMapFrom<KoiEntity>
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<KoiEntity, GetAllActiveAuctionsResponse>();

    }
}
