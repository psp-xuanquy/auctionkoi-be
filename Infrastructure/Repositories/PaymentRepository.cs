using AutoMapper;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Persistences;
using KoiAuction.Infrastructure.Repositories;

namespace KoiAuction.Infrastructure.Repositories
{
    public class PaymentRepository : RepositoryBase<PaymentEntity, PaymentEntity, ApplicationDbContext>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
