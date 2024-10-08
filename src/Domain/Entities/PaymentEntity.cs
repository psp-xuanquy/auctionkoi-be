using KoiAuction.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KoiAuction.Domain.Entities
{
    [Table("Payment")]
    public class PaymentEntity : BaseEntity
    {
        public required string TransactionID { get; set; }
        public TransactionStatus PaymentStatus { get; set; }
        public DateTimeOffset PaymentDate { get; set; }

        [ForeignKey("TransactionID")]
        public virtual TransactionEntity? Transaction { get; set; }
    }
}
