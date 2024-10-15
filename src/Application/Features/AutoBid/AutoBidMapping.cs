//using Application.Features.AutoBid.Queries;
//using AutoMapper;
//using KoiAuction.Application.Features.AutoBid.Commands.Create;
//using KoiAuction.Domain.Entities;

//namespace KoiAuction.Application.Features.AutoBid
//{
//    public static class AutoBidMapping
//    {
//        public static GetAutoBidResponse MapToAutoBidModel(this AutoBidEntity entity, IMapper mapper)
//        => mapper.Map<GetAutoBidResponse>(entity);

//        public static List<GetAutoBidResponse> MapToAutoBidModelList(this IEnumerable<AutoBidEntity> listentity, IMapper mapper)
//        => listentity.Select(x => x.MapToAutoBidModel(mapper)).ToList();

//        public static AutoBidEntity MappingCreateAutoBid(this CreateAutoBidCommand command, IMapper mapper)
//        => mapper.Map<AutoBidEntity>(command);
//    }
//}
