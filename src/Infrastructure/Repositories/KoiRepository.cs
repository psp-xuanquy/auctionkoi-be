using AutoMapper;
using Domain.Enums;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KoiAuction.Domain.Repositories
{
    public class KoiRepository : RepositoryBase<KoiEntity, KoiEntity, ApplicationDbContext>, IKoiRepository
    {
        private readonly ApplicationDbContext _context;

        public KoiRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<KoiEntity>> GetActiveAuctions(CancellationToken cancellationToken)
        {
            return await _context.Kois
                .Where(k => k.AuctionStatus == AuctionStatus.OnGoing)
                .ToListAsync(cancellationToken);
        }
    }
}
