using System.Threading;
using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<TransactionEntity, TransactionEntity, ApplicationDbContext>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TransactionEntity>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .Include(t => t.Bid)  
                .Include(t => t.Koi)
                .Include(t => t.AuctionHistory)
                .ToListAsync(cancellationToken);
        }
    }
}
