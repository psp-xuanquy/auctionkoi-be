using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using MediatR;

namespace AuctionAuction.Application.Features.Auction.Commands.Update
{
    public class UpdateAuctionCommand : IRequest<string>, IMapFrom<AuctionEntity>
    {
        public string? ID { get; set; }
        public string? AuctionMethodID { get; set; }
        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }
        public bool AllowAutoBid { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAuctionCommand, AuctionEntity>();

        }
    }
}
