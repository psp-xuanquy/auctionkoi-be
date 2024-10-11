using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.Koi.Queries.Filter
{
    public class FilterKoiQuery : IRequest<List<GetKoiResponse>>, IQuery
    {
        public string? Name { get; set; }
        public double? MinLength { get; set; }
        public double? MaxLength { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? BreederId { get; set; }
        public Sex? Sex { get; set; } 
        public double? MinRating { get; set; } 
        public double? MaxRating { get; set; }

        public FilterKoiQuery(string? name, double? minLength, double? maxLength,
                              int? minAge, int? maxAge, decimal? minPrice,
                              decimal? maxPrice, string? breederId,
                              Sex? sex, double? minRating, double? maxRating)
        {
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
            MinAge = minAge;
            MaxAge = maxAge;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            BreederId = breederId;
            Sex = sex;
            MinRating = minRating;
            MaxRating = maxRating;
        }
    }
}
