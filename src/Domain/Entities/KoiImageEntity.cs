using KoiAuction.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("KoiImage")]
    public class KoiImageEntity : BaseEntity
    {   
        public string? Url { get; set; }     

        [ForeignKey("KoiID")]
        public string? KoiID { get; set; }
        public virtual KoiEntity? Koi { get; set; }
    }
}
