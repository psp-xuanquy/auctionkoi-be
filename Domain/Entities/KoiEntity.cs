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
        public required string AuctionID { get; set; }
        public required string BreederID { get; set; }
        public string? Name { get; set; }
        public Sex Sex { get; set; }
        public double Length { get; set; }
        public int Age { get; set; }
        public decimal InitialPrice { get; set; }
        public double Rating { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey("AuctionID")]
        public virtual AuctionEntity? Auction { get; set; }

        [ForeignKey("BreederID")]
        public virtual AspNetUser? Breeder { get; set; }

        public virtual ICollection<AutoBidEntity>? AutoBids { get; set; }
        public virtual ICollection<BidEntity>? Bids { get; set; }

        public virtual ICollection<KoiMediaEntity>? KoiMedias { get; set; }
        public virtual ICollection<TransactionEntity>? Transactions { get; set; }
    }
}
