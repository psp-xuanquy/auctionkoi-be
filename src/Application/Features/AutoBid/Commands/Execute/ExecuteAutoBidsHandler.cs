using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Features.AutoBid.Commands.Create;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Infrastructure.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.AutoBid.Commands.Execute
{
    public class ExecuteAutoBidsHandler : IRequestHandler<ExecuteAutoBidCommand, string>
    {
        private readonly IAutoBidRepository _autoBidRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public ExecuteAutoBidsHandler(IAutoBidRepository autoBidRepository, IBidRepository bidRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _autoBidRepository = autoBidRepository;
            _bidRepository = bidRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(ExecuteAutoBidCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new NotFoundException("Please log in again");
            }

            var autoBids = await _autoBidRepository.GetAutoBidsByKoiIdAsync(b => b.KoiID == request.KoiId && b.DeletedBy == null, cancellationToken);
            if (autoBids == null || !autoBids.Any())
            {
                throw new NotFoundException("No AutoBids found for the specified Koi.");
            }

            foreach (var autoBid in autoBids)
            {
                var latestBids = await _bidRepository.GetBidsByKoiIdAsync(b => b.KoiID == request.KoiId && b.DeletedBy == null, cancellationToken);
                var latestBid = latestBids.OrderByDescending(b => b.BidTime).FirstOrDefault();

                if (latestBid == null || latestBid.BidAmount < autoBid.MaxBid)
                {
                    var newBidAmount = latestBid != null ? latestBid.BidAmount + autoBid.IncrementAmount : autoBid.IncrementAmount;

                    var bid = new BidEntity
                    {
                        KoiID = request.KoiId, 
                        BidderID = autoBid.BidderID,
                        BidAmount = newBidAmount,
                        BidTime = DateTimeOffset.UtcNow,
                        IsAutoBid = true,
                        MaxBidPrice = autoBid.MaxBid,
                        IncrementAmount = autoBid.IncrementAmount
                    };

                    _bidRepository.Add(bid);
                    await _bidRepository.UnitOfWork.SaveChangesAsync();
                }
            }

            return "Auto Bids executed successfully";
        }
    }
}
