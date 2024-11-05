using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace KN_EXE201.Application.Features.Category.Queries.GetById
{
    public class GetKoiFarmBreederByIdQuery : IRequest<GetAllKoiFarmBreederResponse>, IQuery
    {
        public string Id;

        public GetKoiFarmBreederByIdQuery(string id)
        {
            Id = id;
        }
    }
}
