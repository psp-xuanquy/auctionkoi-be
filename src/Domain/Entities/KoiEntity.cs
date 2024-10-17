using Domain.Enums;
using KoiAuction.Domain.Entities.Base;
using KoiAuction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("Koi")]
    public class KoiEntity : BaseEntity
    { 
        public string? Name { get; set; }
        public Sex Sex { get; set; }
        public double Size { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public Variety Variety { get; set; }
        public decimal InitialPrice { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? RequestResponse { get; set; }
        public AuctionRequestStatus AuctionRequestStatus { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool AllowAutoBid { get; set; }

        [ForeignKey("AuctionMethodID")]
        public required string AuctionMethodID { get; set; }
        public virtual AuctionMethodEntity? AuctionMethod { get; set; }

        [ForeignKey("BreederID")]
        public required string BreederID { get; set; }
        public virtual UserEntity? Breeder { get; set; }

        public virtual ICollection<AutoBidEntity>? AutoBids { get; set; }
        public virtual ICollection<BidEntity>? Bids { get; set; }
        public virtual ICollection<KoiImageEntity>? KoiImages { get; set; }
        public virtual ICollection<TransactionEntity>? Transactions { get; set; }
    }
}
