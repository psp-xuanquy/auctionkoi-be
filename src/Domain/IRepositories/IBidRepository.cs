using System.Linq.Expressions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface IBidRepository : IEFRepository<BidEntity, BidEntity>
    {
        Task<IEnumerable<BidEntity>> GetBidsByKoiIdAsync(Expression<Func<BidEntity, bool>> filterExpression, CancellationToken cancellationToken = default);
    }
}
