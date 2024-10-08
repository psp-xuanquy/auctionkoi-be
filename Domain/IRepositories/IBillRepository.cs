using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Domain.IRepositories
{
    public interface IBillRepository : IEFRepository<BillEntity, BillEntity>
    {
        
    }
}
