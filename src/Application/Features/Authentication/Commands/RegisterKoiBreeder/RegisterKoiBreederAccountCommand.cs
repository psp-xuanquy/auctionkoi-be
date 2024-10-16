using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using KoiAuction.Application.Common.Mappings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Commands.RegisterKoiBreeder
{
    public class RegisterKoiBreederAccountCommand : IRequest<string>, IMapFrom<KoiBreederEntity>
    {
        public string? KoiFarmName { get; set; }
        public string? KoiFarmDescription { get; set; }
        public string? KoiFarmLocation { get; set; }
        public string? KoiFarmImage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterKoiBreederAccountCommand, KoiBreederEntity>();
        }
    }
}
