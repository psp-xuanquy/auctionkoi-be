using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Commands.Create
{
    public class CreateKoiHandler : IRequestHandler<CreateKoiCommand, string>
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

        public async Task<string> Handle(CreateKoiCommand request, CancellationToken cancellationToken)
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

            var koiEntity = _mapper.Map<KoiEntity>(request);
            _koiRepository.Add(koiEntity);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return "Create Koi success";
        }
    }
}
