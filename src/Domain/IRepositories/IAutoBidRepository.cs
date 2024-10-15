using System.Linq.Expressions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface IAutoBidRepository : IEFRepository<AutoBidEntity, AutoBidEntity>
    {       
    }
}
