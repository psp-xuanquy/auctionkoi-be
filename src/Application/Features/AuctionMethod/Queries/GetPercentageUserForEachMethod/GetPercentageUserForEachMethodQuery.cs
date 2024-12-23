﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class GetPercentageUserForEachMethodQuery : IRequest<List<GetPercentageUserForEachMethodResponse>>, IQuery
{
    public int Year { get; set; }
    public int Month { get; set; }

    public GetPercentageUserForEachMethodQuery(int year, int month)
    {
        Year = year;
        Month = month;
    }
}
