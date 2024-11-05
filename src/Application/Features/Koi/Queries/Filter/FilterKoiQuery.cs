using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Koi.Queries.GetAll;
using KoiAuction.Domain.Enums;
using MediatR;

namespace Application.Features.Koi.Queries.Filter;
public class FilterKoiQuery : IRequest<List<KoiResponse>>
{
    public string? Name { get; }
    public double? MinLength { get; }
    public double? MaxLength { get; }
    public int? MinAge { get; }
    public int? MaxAge { get; }
    public decimal? MinPrice { get; }
    public decimal? MaxPrice { get; }
    public string? BreederName { get; }
    public string? AuctionMethodName { get; }
    public Sex? Sex { get; }

    public FilterKoiQuery(string? name, double? minLength, double? maxLength, int? minAge, int? maxAge,
                          decimal? minPrice, decimal? maxPrice, string? breederName, string? auctionMethodName, Sex? sex)
    {
        Name = name;
        MinLength = minLength;
        MaxLength = maxLength;
        MinAge = minAge;
        MaxAge = maxAge;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        BreederName = breederName;
        AuctionMethodName = auctionMethodName;
        Sex = sex;
    }
}
