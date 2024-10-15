using AutoMapper;
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
        public KoiRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            
        }
    }
}
