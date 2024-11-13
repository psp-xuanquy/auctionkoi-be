using Application.Features.Authentication.Queries.GetAll;
using Application.Features.Transaction.Queries.GetAll;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace KoiAuction.Application.Transaction.Queries.GetAll
{
    public class GetAllTransactionsQuery : IRequest<List<GetAllTransactionsResponse>>, IQuery
    {
        public GetAllTransactionsQuery()
        {

        }
    }
}
