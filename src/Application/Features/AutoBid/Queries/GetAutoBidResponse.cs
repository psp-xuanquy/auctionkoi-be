//using AutoMapper;
//using KoiAuction.Application.Common.Mappings;
//using KoiAuction.Domain.Entities;
//using KoiAuction.Domain.Enums;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Features.AutoBid.Queries
//{
//    public class GetAutoBidResponse : IMapFrom<AutoBidEntity>
//    {
//        public string? ID { get; set; }
//        public required string KoiID { get; set; }
//        public required string BidderID { get; set; }
//        public decimal MaxBid { get; set; }
//        public DateTimeOffset BidTime { get; set; }

//        public void Mapping(Profile profile)
//        {
//            profile.CreateMap<AutoBidEntity, GetAutoBidResponse>();
//        }
//    }
//}
