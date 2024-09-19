using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("Payment")]
public class Payment : BaseAuditableEntity
{
    public required string TransactionID { get; set; }
    public bool PaymentStatus { get; set; }
    public DateTimeOffset PaymentDate { get; set; }

    [ForeignKey("TransactionID")]
    public virtual Transaction? Transaction { get; set; }
}

