using KoiAuction.Domain.Entities.Base;
using KoiAuction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("Blog")]
    public class BlogEntity : BaseEntity
    {     
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? UrlImage { get; set; }
        public DateTime? PostedDate { get; set; }     
        [ForeignKey("AuthorID")]
        public string? AuthorID { get; set; }
        public virtual UserEntity? Author { get; set; }
    }
}
