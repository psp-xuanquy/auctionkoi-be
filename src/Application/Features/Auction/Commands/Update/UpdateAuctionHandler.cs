using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionAuction.Application.Features.Auction.Commands.Update
{
    public class UpdateAuctionHandler : IRequestHandler<UpdateAuctionCommand, string>
    {
        private readonly IAuctionRepository _AuctionRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public UpdateAuctionHandler(IAuctionRepository AuctionRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _AuctionRepository = AuctionRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(UpdateAuctionCommand request, CancellationToken cancellationToken)
        {
            var Auction = await _AuctionRepository.FindAsync(x => x.ID == request.ID && x.DeletedTime == null, cancellationToken);
            if (Auction == null)
            {
                throw new NotFoundException("Auction not found");
            }

            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new NotFoundException("Please log in again");
            }

            var auctionUpdate = _mapper.Map(request, Auction);
            if (auctionUpdate != null)
            {
                auctionUpdate.LastUpdatedBy = user.UserName;
                auctionUpdate.LastUpdatedTime = DateTimeOffset.Now;

                _AuctionRepository.Update(auctionUpdate);
                await _AuctionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return "Auction updated successfully";
            }

            return "Error occurred while updating the AuctionEntity";
        }
    }
}
