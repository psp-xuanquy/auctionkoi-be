using Application.Features.Bid.Queries;
using AutoMapper;
using KoiAuction.Application.Features.Bid.Commands.Create;
using KoiAuction.Domain.Entities;

namespace KoiAuction.Application.Features.Bid
{
    public static class BidMapping
    {
        public static GetBidResponse MapToBidModel(this BidEntity entity, IMapper mapper)
        => mapper.Map<GetBidResponse>(entity);

        public static List<GetBidResponse> MapToBidModelList(this IEnumerable<BidEntity> listentity, IMapper mapper)
        => listentity.Select(x => x.MapToBidModel(mapper)).ToList();

        public static BidEntity MappingCreateBid(this CreateBidCommand command, IMapper mapper)
        => mapper.Map<BidEntity>(command);


    }
}
