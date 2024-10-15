//using AutoMapper;
//using KoiAuction.Application.Common.Mappings;
//using KoiAuction.Domain.Entities;
//using KoiAuction.Domain.Enums;
//using MediatR;

//namespace KoiAuction.Application.Features.Bid.Commands.Create
//{
//    public class CreateBidCommand : IRequest<string>, IMapFrom<BidEntity>
//    {
//        public required string KoiID { get; set; }
//        public required string BidderID { get; set; }
//        public decimal BidAmount { get; set; }
//        //public DateTimeOffset BidTime { get; set; }
//        //public bool IsWinningBid { get; set; }
//        //public bool IsLatest { get; set; }
//        //public DateTimeOffset ExpireDate { get; set; }
//        //public bool IsAutoBid { get; set; }
//        //public decimal MaxBidPrice { get; set; }
//        //public decimal IncrementAmount { get; set; }

//        public void Mapping(Profile profile)
//        {
//            profile.CreateMap<CreateBidCommand, BidEntity>();
//        }
//    }
//}
