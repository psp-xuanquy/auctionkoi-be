using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Features.AutoBid.Commands.Create;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.AutoBid.Commands.Create
{
    public class CreateAutoBidHandler : IRequestHandler<CreateAutoBidCommand, string>
    {
        private readonly IAutoBidRepository _autoBidRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public CreateAutoBidHandler(IAutoBidRepository autoBidRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _autoBidRepository = autoBidRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(CreateAutoBidCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new NotFoundException("Please log in again");
            }

            var autoBid = _mapper.Map<AutoBidEntity>(request);
            if (autoBid != null)
            {
                autoBid.BidTime = DateTimeOffset.UtcNow;
                autoBid.CreatedBy = user.UserName;
                autoBid.CreatedTime = DateTimeOffset.UtcNow;

                _autoBidRepository.Add(autoBid);
                await _autoBidRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return "AutoBid added successfully";
            }

            return "Error occurred while adding the AutoBidEntity";
        }
    }
}
