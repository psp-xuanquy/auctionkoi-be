using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class GetRevenueForEachMethodResponse : IMapFrom<AuctionMethodEntity>
{
    public string AuctionMethodId { get; set; }
    public string AuctionMethodName { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal[] MonthlyRevenue { get; set; } = new decimal[12];
    public string[] MonthlyLabels { get; set; } = new string[12];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuctionMethodEntity, GetRevenueForEachMethodResponse>();
            
    }
}
