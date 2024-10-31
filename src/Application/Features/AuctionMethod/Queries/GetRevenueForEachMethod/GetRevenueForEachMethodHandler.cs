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

        var revenueData = auctionMethods.Select(method =>
        {
            var monthlyRevenue = new decimal[12];
            var monthlyLabels = new string[12];

            for (int i = 0; i < 12; i++)
            {
                monthlyLabels[i] = $"Tháng {i + 1}";
            }

            foreach (var transaction in transactions.Where(t => t.Bid?.Koi?.AuctionMethodID == method.ID))
            {
                if (transaction.TransactionDate.HasValue && transaction.TransactionDate.Value.Year == request.Year)
                {
                    var month = transaction.TransactionDate.Value.Month - 1; // Tháng bắt đầu từ 0
                    if (month >= 0 && month < 12)
                    {
                        monthlyRevenue[month] += transaction.TotalAmount;
                    }
                }
            }

            var totalRevenue = monthlyRevenue.Sum();

            return new GetRevenueForEachMethodResponse
            {
                AuctionMethodId = method.ID,
                AuctionMethodName = method.Name,
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                MonthlyLabels = monthlyLabels 
            };
        }).ToList();

        return revenueData;
    }
}
