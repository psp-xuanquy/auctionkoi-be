using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("Blog")]
public class Blog : BaseAuditableEntity
{
    public required string AuthorID { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTimeOffset PostedDate { get; set; }
    public bool Status { get; set; }

    [ForeignKey("AuthorID")]
    public virtual ApplicationUser? Author { get; set; }
}

