using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("Bill")]
public class Bill : BaseAuditableEntity
{
    public required string CustomerID { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset PaymentDate { get; set; }

    [ForeignKey("CustomerID")]
    public virtual ApplicationUser? Customer { get; set; }
}

