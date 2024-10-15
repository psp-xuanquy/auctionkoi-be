//using AutoMapper;
//using KoiAuction.Application.Common.Interfaces;
//using KoiAuction.Domain.Entities;
//using KoiAuction.Domain.Common.Exceptions;
//using KoiAuction.Domain.IRepositories;
//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;

//namespace KoiAuction.Application.Features.Koi.Commands.Create
//{
//    public class CreateKoiHandler : IRequestHandler<CreateKoiCommand, string>
//    {
//        private readonly IKoiRepository _koiRepository;
//        private readonly IMapper _mapper;
//        private readonly ICurrentUserService _currentUserService;
//        private readonly IUserRepository _userRepository;

//        public CreateKoiHandler(IKoiRepository koiRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
//        {
//            _koiRepository = koiRepository;
//            _mapper = mapper;
//            _currentUserService = currentUserService;
//            _userRepository = userRepository;
//        }

//        public async Task<string> Handle(CreateKoiCommand request, CancellationToken cancellationToken)
//        {
//            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
//            if (user == null)
//            {
//                throw new NotFoundException("Please log in again");
//            }

//            var koi = _mapper.Map<KoiEntity>(request);
//            if (koi != null)
//            {
//                koi.CreatedBy = user.UserName;
//                koi.CreatedTime = DateTimeOffset.Now;

//                _koiRepository.Add(koi);
//                await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

//                return "Koi added successfully";
//            }

//            return "Error occurred while adding the KoiEntity";
//        }
//    }
//}
