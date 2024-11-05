using KoiAuction.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
       [Table("AuctionMethod")]
    public class AuctionMethodEntity : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
