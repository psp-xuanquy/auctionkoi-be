using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class BlogRepository : RepositoryBase<BlogEntity, BlogEntity, ApplicationDbContext>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
