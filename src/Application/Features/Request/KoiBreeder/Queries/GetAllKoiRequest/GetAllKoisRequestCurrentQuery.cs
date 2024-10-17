using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
public class GetAllKoisRequestCurrentQuery : IRequest<List<GetAllKoisRequestCurrentResponse>>, IQuery
{
    public GetAllKoisRequestCurrentQuery()
    {

    }
}
