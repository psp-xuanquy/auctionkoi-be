using KoiAuction.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("Bill")]
    public class BillEntity : BaseEntity
    {
        public required string CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset PaymentDate { get; set; }

        [ForeignKey("CustomerID")]
        public virtual AspNetUser? Customer { get; set; }
    }
}
