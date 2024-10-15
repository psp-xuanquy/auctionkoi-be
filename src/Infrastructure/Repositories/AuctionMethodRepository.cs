using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class AuctionMethodRepository : RepositoryBase<AuctionMethodEntity, AuctionMethodEntity, ApplicationDbContext>, IAuctionMethodRepository
    {
        public AuctionMethodRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
