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
        private readonly IMapper _mapper;

        public AutoBidRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AutoBidEntity>> GetAutoBidsForKoi(string koiId, CancellationToken cancellationToken = default)
        {
            return await _context.AutoBids
                .Where(ab => ab.KoiID == koiId && ab.Kois.AllowAutoBid == true && ab.DeletedTime == null)
                .ToListAsync(cancellationToken);
        }
    }
}
