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

namespace KoiAuction.Application.Features.Auction
{
    public class GetAuctionResponse : IMapFrom<AuctionEntity>
    {
        public string? ID { get; set; }
        public string? AuctionMethodID { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
        public bool AllowAutoBid { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuctionEntity, GetAuctionResponse>();
        }
    }
}
