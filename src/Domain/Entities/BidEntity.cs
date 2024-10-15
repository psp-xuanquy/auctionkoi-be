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
        public decimal BidAmount { get; set; }
        public bool IsWinningBid { get; set; }
        public DateTimeOffset BidTime { get; set; }
        public bool IsAutoBid { get; set; }

        [ForeignKey("KoiID")]
        public required string KoiID { get; set; }
        public virtual KoiEntity? Koi { get; set; }

        [ForeignKey("BidderID")]
        public required string BidderID { get; set; }
        public virtual UserEntity? Bidder { get; set; }
    }
}
