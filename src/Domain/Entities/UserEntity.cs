using Domain.Entities;
using Domain.Enums;
using KoiAuction.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities
{
    [Table("User")]
    public class UserEntity : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool Status { get; set; } = true;
        public string? UrlAvatar { get; set; }
        public decimal Balance { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string LastUpdatedBy { get; set; } = string.Empty;
        public string? DeletedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("GETDATE()")]
        public DateTime? CreatedTime { get; set; } = DateTime.Now;
        public DateTime? DeletedTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpires { get; set; }

        public virtual ICollection<TransactionEntity>? Transactions { get; set; }
        public virtual ICollection<KoiBreederEntity>? KoiBreeders { get; set; }
        public virtual ICollection<BlogEntity>? Blogs { get; set; }
        public virtual ICollection<KoiEntity>? Kois { get; set; }
        public virtual ICollection<BidEntity>? Bids { get; set; }
        public virtual ICollection<AutoBidEntity>? AutoBids { get; set; }
        public virtual ICollection<NotificationEntity>? Notifications { get; set; }
    }
}
