using KoiAuction.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("Bid")]
    public class BidEntity : BaseEntity
    {
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

        [ForeignKey("KoiID")]
        public virtual KoiEntity? Koi { get; set; }

        [ForeignKey("BidderID")]
        public virtual AspNetUser? Bidder { get; set; }
    }
}
