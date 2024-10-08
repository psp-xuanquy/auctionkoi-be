using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class KoiMediaRepository : RepositoryBase<KoiMediaEntity, KoiMediaEntity, ApplicationDbContext>, IKoiMediaRepository
    {
        public KoiMediaRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
