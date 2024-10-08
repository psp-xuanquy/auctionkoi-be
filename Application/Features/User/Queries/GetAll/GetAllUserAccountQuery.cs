using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace KoiAuction.Application.User.Queries.GetAll
{
    public class GetAllUserAccountQuery : IRequest<List<GetUserAccountResponse>>, IQuery
    {
        public GetAllUserAccountQuery()
        {

        }
    }
}
