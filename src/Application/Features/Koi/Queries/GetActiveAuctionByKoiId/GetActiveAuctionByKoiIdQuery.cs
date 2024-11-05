using Application.Features.Koi;
using Application.Features.KoiBreeder.Queries.GetAllKoiFarmBreeder;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace KN_EXE201.Application.Features.Koi.Queries.GetActiveAuctionByKoiId
{
    public class GetActiveAuctionByKoiIdQuery : IRequest<KoiResponse>, IQuery
    {
        public string Id;

        public GetActiveAuctionByKoiIdQuery(string id)
        {
            Id = id;
        }
    }
}
