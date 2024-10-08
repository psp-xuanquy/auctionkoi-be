using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Infrastructure.Repositories
{
    public class BillRepository : RepositoryBase<BillEntity, BillEntity, ApplicationDbContext>, IBillRepository
    {
        public BillRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {

        }
    }
}
