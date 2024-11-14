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
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IKoiRepository KoiRepository => _serviceProvider.GetRequiredService<IKoiRepository>();

        public IBidRepository BidRepository => _serviceProvider.GetRequiredService<IBidRepository>();

        public IUserRepository UserRepository => _serviceProvider.GetRequiredService<IUserRepository>();

        public ITransactionRepository TransactionRepository => _serviceProvider.GetRequiredService<ITransactionRepository>();
    }
}
