using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Features.Auction.Commands.Create;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionAuction.Application.Features.Auction.Commands.Create
{
    public class CreateAuctionHandler : IRequestHandler<CreateAuctionCommand, string>
    {
        private readonly IAuctionRepository _AuctionRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public CreateAuctionHandler(IAuctionRepository AuctionRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _AuctionRepository = AuctionRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new NotFoundException("Please log in again");
            }

            var auction = _mapper.Map<AuctionEntity>(request);
            if (auction != null)
            {
                auction.CreatedBy = user.UserName;
                auction.CreatedTime = DateTimeOffset.Now;

                _AuctionRepository.Add(auction);
                await _AuctionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return "Auction added successfully";
            }

            return "Error occurred while adding the AuctionEntity";
        }
    }
}
