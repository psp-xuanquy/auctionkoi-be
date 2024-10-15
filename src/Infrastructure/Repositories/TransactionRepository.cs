using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;

namespace KoiAuction.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<TransactionEntity, TransactionEntity, ApplicationDbContext>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
