using KoiAuction.Domain.Entities.Base;
using KoiAuction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("Auction")]
    public class AuctionEntity : BaseEntity
    {
        public required string AuctionMethodID { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
        public bool AllowAutoBid { get; set; }
        public Status Status { get; set; } = Status.Pending;

        [ForeignKey("AuctionMethodID")]
        public virtual AuctionMethodEntity? AuctionMethod { get; set; }

        public virtual ICollection<KoiEntity>? Kois { get; set; }
        public virtual ICollection<BidEntity>? Bids { get; set; }
    }
}
