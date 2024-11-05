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
public class GetPercentageUserForEachMethodHandler : IRequestHandler<GetPercentageUserForEachMethodQuery, List<GetPercentageUserForEachMethodResponse>>
{
    private readonly IAuctionMethodRepository _auctionMethodRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository; // Để lấy tổng số người dùng

    public GetPercentageUserForEachMethodHandler(
        IAuctionMethodRepository auctionMethodRepository,
        ITransactionRepository transactionRepository,
        IUserRepository userRepository)
    {
        _auctionMethodRepository = auctionMethodRepository;
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
    }

    public async Task<List<GetPercentageUserForEachMethodResponse>> Handle(GetPercentageUserForEachMethodQuery request, CancellationToken cancellationToken)
    {
        var auctionMethods = await _auctionMethodRepository.FindAllAsync(cancellationToken);
        if (auctionMethods is null || !auctionMethods.Any())
        {
            throw new NotFoundException("No auction methods found.");
        }

        var transactions = await _transactionRepository.FindAllAsync(cancellationToken);
        var filteredTransactions = transactions
            .Where(t => t.TransactionDate.HasValue
                        && t.TransactionDate.Value.Year == request.Year
                        && t.TransactionDate.Value.Month == request.Month)
            .ToList();

        //var totalUsers = await _userRepository.GetTotalUsersAsync(cancellationToken);
        var totalUsers = filteredTransactions
            .Select(t => t.Bid?.BidderID)
            .Distinct()
            .Count();

        var percentageData = auctionMethods.Select(method =>
        {
            var numberUsers = filteredTransactions
                .Where(t => t.Bid?.Koi?.AuctionMethodID == method.ID)
                .Select(t => t.Bid?.BidderID)
                .Distinct()
                .Count();

            return new GetPercentageUserForEachMethodResponse
            {
                AuctionMethodId = method.ID,
                AuctionMethodName = method.Name,
                NumberUsers = numberUsers,
                Percentage = totalUsers > 0 ? (decimal)numberUsers / totalUsers * 100 : 0
            };
        }).ToList();

        return percentageData;
    }        
}
