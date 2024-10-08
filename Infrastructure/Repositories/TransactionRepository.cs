using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<TransactionEntity, TransactionEntity, ApplicationDbContext>, ITransactionRepository
    {
        public TransactionRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
