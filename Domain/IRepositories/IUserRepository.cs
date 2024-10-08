using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface IUserRepository : IEFRepository<AspNetUser, AspNetUser>
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        string GeneratePassword();
    }
}
