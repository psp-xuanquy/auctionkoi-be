using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class GetRevenueForAllMethodsResponse : IMapFrom<AuctionMethodEntity>
{
    public decimal TotalRevenue { get; set; }
    public List<MonthlyRevenueResponse> MonthlyRevenueList { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuctionMethodEntity, GetRevenueForAllMethodsResponse>();
            
    }
}
