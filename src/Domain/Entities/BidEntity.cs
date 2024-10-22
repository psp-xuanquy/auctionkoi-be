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

        public BidEntity(string koiId, decimal bidAmount, string bidderId, decimal userBalance, decimal initialPrice) : this()
        {
            KoiID = koiId ?? throw new ArgumentNullException(nameof(koiId));

            if (bidAmount < initialPrice)
                throw new ArgumentException($"Bid amount must be at least the initial price of {initialPrice:C}");

            if (bidAmount > userBalance)
                throw new ArgumentException("Bid amount cannot exceed the user's balance.");

            BidAmount = bidAmount;
            BidderID = bidderId ?? throw new ArgumentNullException(nameof(bidderId));
        }

        public void MarkAsWinningBid()
        {
            IsWinningBid = true;
        }
    }
}
