using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface ITransactionRepository : IEFRepository<TransactionEntity, TransactionEntity>
    {
        Task<List<TransactionEntity>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
    }
}
