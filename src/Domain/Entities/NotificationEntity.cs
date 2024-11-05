using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Entities.Base;

namespace Domain.Entities;
[Table("Notification")]
public class NotificationEntity : BaseEntity
{
    public string Message { get; set; }
    public bool MarkAsRead { get; set; }

    [ForeignKey("UserID")]
    public string? UserID { get; set; }
    public virtual UserEntity? User { get; set; }
}
