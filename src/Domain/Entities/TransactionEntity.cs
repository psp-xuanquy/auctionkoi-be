using Domain.Entities;
using Domain.Enums;
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
        public TransactionType TransactionType { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? TransactionDate { get; set; }  
        public string PaymentMethod { get; set; } = "Banking";
        public decimal CommissionRate { get; set; }

        [ForeignKey("BidID")]
        public string? BidID { get; set; }
        public virtual BidEntity? Bid { get; set; }

        [ForeignKey("KoiID")]
        public string? KoiID { get; set; }
        public virtual KoiEntity? Koi { get; set; }

        [ForeignKey("AuctionHistoryID")]
        public string? AuctionHistoryId { get; set; }
        public virtual AuctionHistory? AuctionHistory { get; set; }
    }
}
