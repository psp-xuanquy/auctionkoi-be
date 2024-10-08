using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class AuctionRepository : RepositoryBase<AuctionEntity, AuctionEntity, ApplicationDbContext>, IAuctionRepository
    {
        public AuctionRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
