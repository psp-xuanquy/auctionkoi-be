using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Domain.Entities.Base
{
    public abstract class BaseEntity : IDisposable
    {
        protected BaseEntity()
        {
            ID = Guid.NewGuid().ToString("N");
            CreatedTime = LastUpdatedTime = DateTimeOffset.Now;
        }

        [Key]
        public string ID { get; set; }

        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DefaultValue("GETDATE()")]
        public DateTimeOffset? CreatedTime { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? LastUpdatedTime { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }

        [NotMapped]
        private bool IsDisposed { get; set; }
        #region Dispose
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!IsDisposed)
            {
                if (isDisposing)
                {
                    DisposeUnmanagedResources();
                }

                IsDisposed = true;
            }
        }

        protected virtual void DisposeUnmanagedResources()
        {
        }

        ~BaseEntity()
        {
            Dispose(isDisposing: false);
        }
        #endregion Dispose
    }
}
