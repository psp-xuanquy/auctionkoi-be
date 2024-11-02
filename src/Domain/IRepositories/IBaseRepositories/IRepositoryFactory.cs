using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Domain.IRepositories;

namespace Domain.IRepositories.IBaseRepositories
{
    public interface IRepositoryFactory
    {
        IKoiRepository KoiRepository { get; }
        IBidRepository BidRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
