using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Bid.Queries
{
    public class GetBidResponse : IMapFrom<BidEntity>
    {
        public string? ID { get; set; }
        public required string KoiID { get; set; }
        public required string BidderID { get; set; }
        public decimal BidAmount { get; set; }
        public DateTimeOffset BidTime { get; set; }
        public bool IsWinningBid { get; set; }
        public bool IsLatest { get; set; }
        public DateTimeOffset ExpireDate { get; set; }
        public bool IsAutoBid { get; set; }
        public decimal MaxBidPrice { get; set; }
        public decimal IncrementAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BidEntity, GetBidResponse>();
        }
    }
}
