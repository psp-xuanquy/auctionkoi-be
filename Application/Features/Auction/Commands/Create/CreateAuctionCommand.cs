using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using MediatR;

namespace KoiAuction.Application.Features.Auction.Commands.Create
{
    public class CreateAuctionCommand : IRequest<string>, IMapFrom<AuctionEntity>
    {
        public required string AuctionMethodID { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
        public bool AllowAutoBid { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAuctionCommand, AuctionEntity>();
        }
    }
}
