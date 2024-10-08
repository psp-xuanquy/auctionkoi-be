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
        public required string AuthorID { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTimeOffset PostedDate { get; set; }
        public Status Status { get; set; }

        [ForeignKey("AuthorID")]
        public virtual AspNetUser? Author { get; set; }
    }
}
