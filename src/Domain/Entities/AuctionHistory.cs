using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Entities.Base;
using KoiAuction.Domain.Enums;

namespace Domain.Entities;
[Table("AuctionHistory")]
public class AuctionHistory : BaseEntity
{
    public DeliveryStatus DeliveryStatus { get; set; }

    public virtual ICollection<TransactionEntity>? Transactions { get; set; }
}
