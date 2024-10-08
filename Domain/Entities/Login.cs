using System.ComponentModel.DataAnnotations.Schema;

namespace KoiAuction.Domain.Entities
{
    [NotMapped]
    public class Login
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
