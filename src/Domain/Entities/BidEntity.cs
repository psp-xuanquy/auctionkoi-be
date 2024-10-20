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
        public DateTime BidTime { get; set; }
        public bool IsAutoBid { get; set; }

        [ForeignKey("KoiID")]
        public string KoiID { get; set; }
        public virtual KoiEntity? Koi { get; set; }

        [ForeignKey("BidderID")]
        public string BidderID { get; set; }
        public virtual UserEntity? Bidder { get; set; }

        public BidEntity()
        {
            BidTime = DateTime.UtcNow;
            IsWinningBid = false;
            IsAutoBid = false;
        }

        public BidEntity(string koiId, decimal bidAmount, string bidderId) : this() 
        {
            KoiID = koiId ?? throw new ArgumentNullException(nameof(koiId));
            BidAmount = bidAmount;
            BidderID = bidderId ?? throw new ArgumentNullException(nameof(bidderId));
        }

        public void MarkAsWinningBid()
        {
            IsWinningBid = true;
        }
    }
}
