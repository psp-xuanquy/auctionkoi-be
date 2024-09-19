using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("Transaction")]
public class Transaction : BaseAuditableEntity
{
    public required string KoiID { get; set; }
    public required string BuyerID { get; set; }
    public required string SellerID { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset TransactionDate { get; set; }
    public bool TransactionStatus { get; set; }
    public bool ShippingStatus { get; set; }

    [ForeignKey("KoiID")]
    public virtual Koi? Koi { get; set; }

    [ForeignKey("BuyerID")]
    public virtual ApplicationUser? Buyer { get; set; }

    [ForeignKey("SellerID")]
    public virtual ApplicationUser? Seller { get; set; }

    public virtual ICollection<Payment>? Payments { get; set; }
}

