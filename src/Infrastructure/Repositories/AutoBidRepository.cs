using System.Linq.Expressions;
using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Infrastructure.Repositories
{
    public class AutoBidRepository : RepositoryBase<AutoBidEntity, AutoBidEntity, ApplicationDbContext>, IAutoBidRepository
    {
        private readonly ApplicationDbContext _context;

        public AutoBidRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<IEnumerable<AutoBidEntity>> GetAutoBidsByKoiIdAsync(Expression<Func<AutoBidEntity, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return await _context.AutoBids
                .Where(filterExpression)
                .ToListAsync(cancellationToken);
        }
    }
}
