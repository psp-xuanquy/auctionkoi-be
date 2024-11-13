using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;

namespace Application.Features.Transaction.Queries.GetAll
{
    public class GetAllTransactionsResponse : IMapFrom<TransactionEntity>
    {
        public string? ID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string PaymentMethod { get; set; } = "Banking";
        public decimal CommissionRate { get; set; }
        public string BidMethod { get; set; }
        public string? KoiName { get; set; }
        public string? BidderEmail { get; set; }
        public decimal BidAmount { get; set; }
        public PaymentStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TransactionEntity, GetAllTransactionsResponse>()
                .ForMember(d => d.KoiName, opt => opt.MapFrom(s => s.Koi.Name)) 
                .ForMember(d => d.BidderEmail, opt => opt.MapFrom(s => s.Bid.Bidder.Email))
                .ForMember(d => d.BidMethod, opt => opt.MapFrom(s => s.Koi.AuctionMethod.Name))
                .ForMember(d => d.BidAmount, opt => opt.MapFrom(s => s.Bid.BidAmount));

            //profile.CreateMap<KoiEntity, GetAllTransactionsResponse>();
        }

    }
}
