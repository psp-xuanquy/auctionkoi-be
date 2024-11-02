using System.Threading.Tasks;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace KoiAuction.Domain.IRepositories
{
    public interface IKoiRepository : IEFRepository<KoiEntity, KoiEntity>
    {
        Task<IEnumerable<KoiEntity>> GetActiveAuctions(CancellationToken cancellationToken);
    }
}
