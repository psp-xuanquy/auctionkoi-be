using System.Linq.Expressions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface IBidRepository : IEFRepository<BidEntity, BidEntity>
    {
        Task<IEnumerable<BidEntity>> GetBidsForKoi(string koiId, CancellationToken cancellationToken = default);
        Task<BidEntity> GetUserBidForKoi(string koiId, string bidderId, CancellationToken cancellationToken = default);
        Task<BidEntity?> GetHighestBidForKoi(string koiId, CancellationToken cancellationToken);
        Task<List<BidEntity>> FindBidsByUserIdAsync(string userId, CancellationToken cancellationToken);
    }
}
