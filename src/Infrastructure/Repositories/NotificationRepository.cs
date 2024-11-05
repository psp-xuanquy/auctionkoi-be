using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace Infrastructure.Repositories;
public class NotificationRepository : RepositoryBase<NotificationEntity, NotificationEntity, ApplicationDbContext>, INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

}
