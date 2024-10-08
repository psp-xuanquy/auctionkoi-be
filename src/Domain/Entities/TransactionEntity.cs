using KoiAuction.Domain.Entities.Base;
using KoiAuction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KoiAuction.Domain.Entities
{
    [Table("Transaction")]
    public class TransactionEntity : BaseEntity
    {
        public required string KoiID { get; set; }
        public required string BuyerID { get; set; }
        public required string SellerID { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public ShippingStatus ShippingStatus { get; set; }

        [ForeignKey("KoiID")]
        public virtual KoiEntity? Koi { get; set; }

        [ForeignKey("BuyerID")]
        public virtual AspNetUser? Buyer { get; set; }

        [ForeignKey("SellerID")]
        public virtual AspNetUser? Seller { get; set; }

        public virtual ICollection<PaymentEntity>? Payments { get; set; }
    }
}
