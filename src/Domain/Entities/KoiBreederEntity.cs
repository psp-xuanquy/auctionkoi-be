using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Entities.Base;

namespace Domain.Entities;
[Table("KoiBreeder")]
public class KoiBreederEntity : BaseEntity
{
    public string? KoiFarmName { get; set; }
    public string? KoiFarmDescription { get; set; }
    public string? KoiFarmLocation { get; set; }
    public string? KoiFarmImage { get; set; }
    public RoleRequestStatus? RoleRequestStatus { get; set; }
    public string? RequestResponse { get; set; }

    [ForeignKey("UserID")]
    public string? UserId { get; set; }
    public virtual UserEntity User { get; set; }
}
