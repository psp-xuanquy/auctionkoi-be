using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Enums;
using MediatR;

namespace KoiAuction.Application.Features.AutoBid.Commands.Execute
{
    public class ExecuteAutoBidCommand : IRequest<string>, IMapFrom<AutoBidEntity>
    {
        public required string KoiId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExecuteAutoBidCommand, AutoBidEntity>();
        }
    }
}
