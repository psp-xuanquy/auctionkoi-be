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
    [Table("AspNetUser")]
    public class AspNetUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? FarmName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool Status { get; set; } = true;
        public decimal Balance { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public string LastUpdatedBy { get; set; } = string.Empty;
        public string? DeletedBy { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("GETDATE()")]
        public DateTimeOffset? CreatedTime { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? DeletedTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? LastUpdatedTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTimeOffset? ResetTokenExpires { get; set; }
        public string? VerificationToken { get; set; }
        public DateTimeOffset? VerificationTokenExpires { get; set; }

        public virtual ICollection<TransactionEntity>? BoughtTransactions { get; set; }
        public virtual ICollection<TransactionEntity>? SoldTransactions { get; set; }
        public virtual ICollection<BillEntity>? Bills { get; set; }
        public virtual ICollection<BlogEntity>? Blogs { get; set; }
        public virtual ICollection<KoiEntity>? Kois { get; set; }
        public virtual ICollection<BidEntity>? Bids { get; set; }
        public virtual ICollection<AutoBidEntity>? AutoBids { get; set; }
    }
}
