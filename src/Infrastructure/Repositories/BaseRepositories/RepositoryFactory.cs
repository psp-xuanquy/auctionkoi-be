using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IRepositories.IBaseRepositories;
using KoiAuction.Domain.IRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories.BaseRepositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RepositoryFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IKoiRepository KoiRepository
        {
            get
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetRequiredService<IKoiRepository>();
            }
        }

        public IBidRepository BidRepository
        {
            get
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetRequiredService<IBidRepository>();
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetRequiredService<IUserRepository>();
            }
        }
    }
}
