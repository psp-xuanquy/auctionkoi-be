    using System.Linq.Expressions;
using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Infrastructure.Repositories
{
    public class BidRepository : RepositoryBase<BidEntity, BidEntity, ApplicationDbContext>, IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<IEnumerable<BidEntity>> GetBidsByKoiIdAsync(Expression<Func<BidEntity, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return await _context.Bids
                .Where(filterExpression)
                .OrderByDescending(b => b.BidAmount)
                .ToListAsync(cancellationToken);
        }
    }
}
