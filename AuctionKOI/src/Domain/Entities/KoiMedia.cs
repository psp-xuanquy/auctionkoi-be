using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionKOI.Domain.Entities;

[Table("KoiMedia")]
public class KoiMedia : BaseAuditableEntity
{
    public required string KoiID { get; set; }
    public string? Url { get; set; }
    public string? UrlType { get; set; }

    [ForeignKey("KoiID")]
    public virtual Koi? Koi { get; set; }
}

