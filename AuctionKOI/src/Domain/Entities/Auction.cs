using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("Auction")]
public class Auction : BaseAuditableEntity
{
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public bool Status { get; set; }

    public virtual ICollection<Koi>? Kois { get; set; }
    public virtual ICollection<Bid>? Bids { get; set; }
}

