using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class GetPercentageUserForEachMethodResponse : IMapFrom<AuctionMethodEntity>
{
    public string AuctionMethodId { get; set; }
    public string AuctionMethodName { get; set; }
    public int NumberUsers { get; set; }
    public decimal Percentage { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuctionMethodEntity, GetPercentageUserForEachMethodResponse>();
            
    }
}
