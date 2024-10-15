using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class KoiImageRepository : RepositoryBase<KoiImageEntity, KoiImageEntity, ApplicationDbContext>, IKoiImageRepository
    {
        public KoiImageRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {         
            
        }
    }
}
