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
        public AutoBidRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            
        }
    }
}
