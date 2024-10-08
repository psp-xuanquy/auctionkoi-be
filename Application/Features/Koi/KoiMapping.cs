using AutoMapper;
using KoiAuction.Application.Features.Koi.Commands.Create;
using KoiAuction.Application.Features.Koi.Queries;
using KoiAuction.Domain.Entities;

namespace KoiAuction.Application.Features.Koi
{
    public static class KoiMapping
    {
        public static GetKoiResponse MapToKoiModel(this KoiEntity entity, IMapper mapper)
        => mapper.Map<GetKoiResponse>(entity);

        public static List<GetKoiResponse> MapToKoiModelList(this IEnumerable<KoiEntity> listentity, IMapper mapper)
        => listentity.Select(x => x.MapToKoiModel(mapper)).ToList();

        public static KoiEntity MappingCreateKoi(this CreateKoiCommand command, IMapper mapper)
        => mapper.Map<KoiEntity>(command);


    }
}
