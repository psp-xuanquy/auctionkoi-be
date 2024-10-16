using AutoMapper;
using Domain.Entities;
using KoiAuction.Application.Common.Mappings;
using MediatR;

namespace Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
public class ResendRegisterKoiBreederCommand : IRequest<string>, IMapFrom<KoiBreederEntity>
{
    public string? KoiBreederID { get; set; }
    public string? KoiFarmName { get; set; }
    public string? KoiFarmDescription { get; set; }
    public string? KoiFarmLocation { get; set; }
    public string? KoiFarmImage { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ResendRegisterKoiBreederCommand, KoiBreederEntity>();
    }
}

