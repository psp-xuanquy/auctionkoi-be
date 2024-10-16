using AutoMapper;
using KoiAuction.Application.Common.Mappings;
using KoiAuction.Domain.Entities;

namespace Application.Features.Authentication.Queries.GetAll
{
    public class GetUserAccountResponse : IMapFrom<UserEntity>
    {
        public string? ID { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserEntity, GetUserAccountResponse>();
        }

    }
}
