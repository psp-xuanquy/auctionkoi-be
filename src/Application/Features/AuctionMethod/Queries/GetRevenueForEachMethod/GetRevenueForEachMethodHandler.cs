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
public class GetRevenueForEachMethodHandler : IRequestHandler<GetRevenueForEachMethodQuery, GetRevenueForAllMethodsResponse>
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


    public async Task<GetRevenueForAllMethodsResponse> Handle(GetRevenueForEachMethodQuery request, CancellationToken cancellationToken)
    {
        var auctionMethods = await _auctionMethodRepository.FindAllAsync(cancellationToken);
        if (auctionMethods == null || !auctionMethods.Any())
        {
            throw new NotFoundException("No auction methods found.");
        }

        var transactions = await _transactionRepository.FindAllAsync(cancellationToken);
        var filteredTransactions = transactions
            .Where(t => t.TransactionDate.HasValue && t.TransactionDate.Value.Year == request.Year)
            .ToList();

        // Aggregate total revenue across all methods
        decimal totalRevenue = filteredTransactions.Sum(t => t.TotalAmount);

        // Calculate monthly revenue for all methods
        var monthlyRevenueList = new List<MonthlyRevenueResponse>();
        for (int month = 1; month <= 12; month++)
        {
            decimal totalMonthlyRevenue = filteredTransactions
                .Where(t => t.TransactionDate.Value.Month == month)
                .Sum(t => t.TotalAmount);

            monthlyRevenueList.Add(new MonthlyRevenueResponse
            {
                Month = month.ToString(),
                Revenue = totalMonthlyRevenue
            });
        }

        return new GetRevenueForAllMethodsResponse
        {
            TotalRevenue = totalRevenue,
            MonthlyRevenueList = monthlyRevenueList
        };
    }
}
