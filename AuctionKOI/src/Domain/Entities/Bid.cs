using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("Bid")]
public class Bid : BaseAuditableEntity
{
    public required string KoiID { get; set; }
    public required string BidderID { get; set; }
    public decimal BidAmount { get; set; }
    public DateTimeOffset BidTime { get; set; }
    public bool IsWinningBid { get; set; }
    public bool IsLatest { get; set; }
    public DateTimeOffset ExpireDate { get; set; }

    [ForeignKey("KoiID")]
    public virtual Koi? Koi { get; set; }

    [ForeignKey("BidderID")]
    public virtual ApplicationUser? Bidder { get; set; }
}

