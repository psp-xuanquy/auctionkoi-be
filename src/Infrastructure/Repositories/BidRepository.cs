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
        private readonly IMapper _mapper;

        public BidRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BidEntity>> GetBidsForKoi(string koiId, CancellationToken cancellationToken = default)
        {
            return await _context.Bids.Where(b => b.KoiID == koiId && b.DeletedTime == null).ToListAsync(cancellationToken);
        }

        public async Task<BidEntity?> GetHighestBidForKoi(string koiId, CancellationToken cancellationToken)
        {
            return await _context.Bids
            .Where(b => b.KoiID == koiId && b.DeletedTime == null)
            .OrderByDescending(b => b.BidAmount) 
            .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<BidEntity> GetUserBidForKoi(string koiId, string bidderId, CancellationToken cancellationToken = default)
        {
            return await _context.Bids.FirstOrDefaultAsync(b => b.KoiID == koiId && b.BidderID == bidderId && b.DeletedTime == null, cancellationToken);
        }
    }
}
