using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using MediatR;

namespace KoiAuction.Application.Features.AutoBid.Commands.Create
{
    public class CreateAutoBidCommand : IRequest<string>, IMapFrom<AutoBidEntity>
    {
        public required string KoiID { get; set; }
        public required string BidderID { get; set; }
        public decimal MaxBid { get; set; }
        public decimal IncrementAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAutoBidCommand, AutoBidEntity>();
        }
    }
}
