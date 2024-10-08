using System.Linq.Expressions;
using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Infrastructure.Repositories
{
    public class AuctionRepository : RepositoryBase<AuctionEntity, AuctionEntity, ApplicationDbContext>, IAuctionRepository
    {
        private readonly ApplicationDbContext _context;

        public AuctionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<AuctionEntity?> GetAuctionWithDetailsAsync(Expression<Func<AuctionEntity, bool>> filterExpression, CancellationToken cancellationToken = default)
        {
            return await _context.Auctions
                .Include(a => a.Kois)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(filterExpression);
        }
    }
}
