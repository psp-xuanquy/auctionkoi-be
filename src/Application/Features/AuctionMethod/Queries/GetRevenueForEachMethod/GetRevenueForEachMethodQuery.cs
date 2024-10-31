﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class GetRevenueForEachMethodQuery : IRequest<List<GetRevenueForEachMethodResponse>>, IQuery
{
    public int Year { get; }

    public GetRevenueForEachMethodQuery(int year)
    {
        Year = year;
    }
}
