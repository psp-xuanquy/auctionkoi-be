using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;

namespace Domain.IRepositories;
public interface INotificationRepository : IEFRepository<NotificationEntity, NotificationEntity>
{
}
