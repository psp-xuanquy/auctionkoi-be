using System.Linq.Expressions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface IAuctionRepository : IEFRepository<AuctionEntity, AuctionEntity>
    {
        Task<AuctionEntity?> GetAuctionWithDetailsAsync(Expression<Func<AuctionEntity, bool>> filterExpression, CancellationToken cancellationToken = default);
    }
}
