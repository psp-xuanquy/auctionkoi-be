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
        public decimal MaxBid { get; set; }
        public decimal IncrementAmount { get; set; }
        public DateTime BidTime { get; set; }

        [ForeignKey("KoiID")]
        public string? KoiID { get; set; }
        public virtual KoiEntity? Kois { get; set; }

        [ForeignKey("BidderID")]
        public string? BidderID { get; set; }
        public virtual UserEntity? Bidder { get; set; }
    }
}
