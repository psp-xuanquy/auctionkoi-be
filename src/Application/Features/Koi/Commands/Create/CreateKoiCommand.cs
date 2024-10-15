//using AutoMapper;
//using KoiAuction.Application.Common.Mappings;
//using KoiAuction.Domain.Entities;
//using KoiAuction.Domain.Enums;
//using MediatR;

//namespace KoiAuction.Application.Features.Koi.Commands.Create
//{
//    public class CreateKoiCommand : IRequest<string>, IMapFrom<KoiEntity>
//    {
//        public string? AuctionID { get; set; }
//        public string? BreederID { get; set; }
//        public string? Name { get; set; }
//        public Sex Sex { get; set; }
//        public double Length { get; set; }
//        public int Age { get; set; }
//        public decimal InitialPrice { get; set; }
//        public double Rating { get; set; }
//        public string? Description { get; set; }
//        public string? ImageUrl { get; set; }

//        public void Mapping(Profile profile)
//        {
//            profile.CreateMap<CreateKoiCommand, KoiEntity>();
//        }
//    }
//}
