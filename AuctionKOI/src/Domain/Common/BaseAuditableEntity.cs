using System.ComponentModel.DataAnnotations;

namespace AuctionKOI.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    [Key]
    [Required]
    public string Id { get; set; }

    protected BaseAuditableEntity()
    {
        Id = Guid.NewGuid().ToString();
        Created = LastModified = DateTime.UtcNow;
    }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTimeOffset Deleted { get; set; }

    public string? DeletedBy { get; set; }
}
