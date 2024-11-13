using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class MonthlyRevenueResponse
{
    public string Month { get; set; }
    public decimal Revenue { get; set; }
}
