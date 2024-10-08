using AutoMapper;
using KoiAuction.Application.Features.Auction;
using KoiAuction.Application.Features.Auction.Commands.Create;
using KoiAuction.Domain.Entities;

namespace AuctionAuction.Application.Features.Auction
{
    public static class AuctionMapping
    {
        public static GetAuctionResponse MapToAuctionModel(this AuctionEntity entity, IMapper mapper)
        => mapper.Map<GetAuctionResponse>(entity);

        public static List<GetAuctionResponse> MapToAuctionModelList(this IEnumerable<AuctionEntity> listentity, IMapper mapper)
        => listentity.Select(x => x.MapToAuctionModel(mapper)).ToList();

        public static AuctionEntity MappingCreateAuction(this CreateAuctionCommand command, IMapper mapper)
        => mapper.Map<AuctionEntity>(command);


    }
}
