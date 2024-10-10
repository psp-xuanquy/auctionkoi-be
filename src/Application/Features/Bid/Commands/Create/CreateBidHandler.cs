using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Features.Bid.Commands.Create;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.Bid.Commands.Create
{
    public class CreateBidHandler : IRequestHandler<CreateBidCommand, string>
    {
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public CreateBidHandler(IBidRepository bidRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _bidRepository = bidRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new NotFoundException("Please log in again");
            }

            var bid = _mapper.Map<BidEntity>(request);
            if (bid != null)
            {
                bid.BidTime = DateTimeOffset.UtcNow;
                bid.IsAutoBid = false;
                bid.CreatedBy = user.UserName;
                bid.CreatedTime = DateTimeOffset.UtcNow;

                _bidRepository.Add(bid);
                await _bidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return "Bid created successfully";
            }

            return "Error occurred while adding the BidEntity";
        }
    }
}
