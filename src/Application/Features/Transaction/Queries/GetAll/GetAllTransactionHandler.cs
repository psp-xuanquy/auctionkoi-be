using MediatR;
using KoiAuction.Domain.IRepositories;
using AutoMapper;
using KoiAuction.Domain.Common.Exceptions;
using Application.Features.Authentication.Queries.GetAll;
using KoiAuction.Infrastructure.Repositories;
using Application.Features.Transaction.Queries.GetAll;

namespace KoiAuction.Application.Transaction.Queries.GetAll
{
    public class GetAllTransactionHandler : IRequestHandler<GetAllTransactionsQuery, List<GetAllTransactionsResponse>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetAllTransactionHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<List<GetAllTransactionsResponse>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {

            var list = await _transactionRepository.FindAllAsync(x => x.DeletedBy == null, cancellationToken);
            if (list is null)
            {
                throw new NotFoundException("Empty list");
            }
            var responseList = list.Select(transaction => new GetAllTransactionsResponse
            {
                ID = transaction.ID,
                TransactionDate = transaction.TransactionDate,
                PaymentMethod = transaction.PaymentMethod,
                CommissionRate = transaction.CommissionRate,
                Status = transaction.Status,
                KoiName = transaction.Koi?.Name,  
                BidderName = transaction.Bid?.Bidder?.UserName, 
                BidAmount = transaction.Bid?.BidAmount ?? 0  
            }).ToList();

            return responseList;
        }
    }
}
