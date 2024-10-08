using KoiAuction.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("AutoBid")]
    public class AutoBidEntity : BaseEntity
    {
        public required string KoiID { get; set; }
        public required string BidderID { get; set; }
        public decimal MaxBid { get; set; }
        public DateTimeOffset BidTime { get; set; }

        [ForeignKey("KoiID")]
        public virtual KoiEntity? Kois { get; set; }

        [ForeignKey("BidderID")]
        public virtual AspNetUser? Bidder { get; set; }
    }
}
