using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("AutoBid")]
public class AutoBid : BaseAuditableEntity
{
    public required string KoiID { get; set; }
    public required string BidderID { get; set; }
    public decimal MaxBid { get; set; }
    public DateTimeOffset BidTime { get; set; }

    [ForeignKey("KoiID")]
    public virtual Koi? Kois { get; set; }

    [ForeignKey("BidderID")]
    public virtual ApplicationUser? Bidder { get; set; }
}

