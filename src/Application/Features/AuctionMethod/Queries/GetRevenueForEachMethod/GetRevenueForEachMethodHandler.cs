using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;

namespace Application.Features.AuctionMethod.Queries.GetRevenueForEachMethod;
public class GetRevenueForEachMethodHandler : IRequestHandler<GetRevenueForEachMethodQuery, List<GetRevenueForEachMethodResponse>>
{
    private readonly IAuctionMethodRepository _auctionMethodRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public GetRevenueForEachMethodHandler(IAuctionMethodRepository auctionMethodRepository,
            ITransactionRepository transactionRepository,
            IMapper mapper)
    {
        _auctionMethodRepository = auctionMethodRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }


    public async Task<List<GetRevenueForEachMethodResponse>> Handle(GetRevenueForEachMethodQuery request, CancellationToken cancellationToken)
    {
        var auctionMethods = await _auctionMethodRepository.FindAllAsync(cancellationToken);
        if (auctionMethods is null || !auctionMethods.Any())
        {
            throw new NotFoundException("No auction methods found.");
        }

        var transactions = await _transactionRepository.FindAllAsync(cancellationToken);
        var filteredTransactions = transactions
            .Where(t => t.TransactionDate.HasValue
                        && t.TransactionDate.Value.Year == request.Year)
            .ToList();

        //var response = auctionMethods.Select(method =>
        //{
        //    var methodTransactions = filteredTransactions
        //        .Where(t => t.Bid?.Koi?.AuctionMethodID == method.ID)
        //        .ToList();

        //    decimal totalRevenue = methodTransactions.Sum(t => t.TotalAmount);

        //    var monthlyRevenue = new Dictionary<string, decimal>();
        //    for (int month = 1; month <= 12; month++)
        //    {
        //        var monthlyTotal = methodTransactions
        //            .Where(t => t.TransactionDate.Value.Month == month)
        //            .Sum(t => t.TotalAmount);

        //        monthlyRevenue.Add($"Month: {month}", monthlyTotal);
        //    }

        //    // Create the response object
        //    return new GetRevenueForEachMethodResponse
        //    {
        //        AuctionMethodId = method.ID,
        //        AuctionMethodName = method.Name,
        //        TotalRevenue = totalRevenue,
        //        MonthlyRevenue = monthlyRevenue
        //    };
        //}).ToList();

        //return response;
        var responseList = new List<GetRevenueForEachMethodResponse>();

        // Duyệt qua từng phương thức đấu giá
        foreach (var method in auctionMethods)
        {
            var methodTransactions = filteredTransactions
                .Where(t => t.Bid?.Koi?.AuctionMethodID == method.ID)
                .ToList();

            // Tính tổng doanh thu của phương thức
            decimal totalRevenue = methodTransactions.Sum(t => t.TotalAmount);

            // Danh sách doanh thu theo từng tháng
            var monthlyRevenueList = new List<MonthlyRevenueResponse>();

            // Duyệt qua từng tháng trong năm
            for (int month = 1; month <= 12; month++)
            {
                // Lọc các giao dịch trong tháng hiện tại
                var monthlyTransactions = methodTransactions
                    .Where(t => t.TransactionDate.Value.Month == month)
                    .ToList();

                decimal totalMonthlyRevenue = monthlyTransactions.Sum(t => t.TotalAmount);

                monthlyRevenueList.Add(new MonthlyRevenueResponse
                {
                    Month = $"Month: {month}",
                    Revenue = totalMonthlyRevenue
                });
            }

            responseList.Add(new GetRevenueForEachMethodResponse
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenueList = monthlyRevenueList
            });
        }

        return responseList;
    }
}
