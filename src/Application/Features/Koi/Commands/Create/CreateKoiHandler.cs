using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.AuctionMethod;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Commands.Create
{
    public class CreateKoiHandler : IRequestHandler<CreateKoiCommand, KoiResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly IKoiRepository _koiRepository;

        public CreateKoiHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository, IKoiRepository koiRepository)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _koiRepository = koiRepository;
        }

        public async Task<KoiResponse> Handle(CreateKoiCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Please login again");
            }

            var existKoi = await _koiRepository.FindAsync(x => x.Name == request.Name, cancellationToken);
            if (existKoi != null)
            {
                throw new DuplicationException("This Koi already exists");
            }

            var koi = new KoiEntity
            {
                Name = request.Name,
                Description = request.Description,
                CreatedBy = user.UserName,
                CreatedTime = DateTime.Now,
                Sex = request.Sex,
                Size = request.Size,
                Age = request.Age,
                Location = request.Location,
                Variety = request.Variety,
                InitialPrice = request.InitialPrice,
                ImageUrl = request.ImageUrl,
                RequestResponse = request.RequestResponse,
                AuctionRequestStatus = request.AuctionRequestStatus,
                AuctionStatus = request.AuctionStatus,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                AllowAutoBid = request.AllowAutoBid,
                AuctionMethodID = request.AuctionMethodID,
                BreederID = request.BreederID
            };


            _koiRepository.Add(koi);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<KoiResponse>(koi);
        }
    }
}
