//using AutoMapper;
//using KoiAuction.Application.Common.Interfaces;
//using KoiAuction.Domain.Common.Exceptions;
//using KoiAuction.Domain.IRepositories;
//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;

//namespace KoiAuction.Application.Features.Koi.Commands.Update
//{
//    public class UpdateKoiHandler : IRequestHandler<UpdateKoiCommand, string>
//    {
//        private readonly IKoiRepository _koiRepository;
//        private readonly IMapper _mapper;
//        private readonly ICurrentUserService _currentUserService;
//        private readonly IUserRepository _userRepository;

//        public UpdateKoiHandler(IKoiRepository koiRepository, IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository)
//        {
//            _koiRepository = koiRepository;
//            _mapper = mapper;
//            _currentUserService = currentUserService;
//            _userRepository = userRepository;
//        }

//        public async Task<string> Handle(UpdateKoiCommand request, CancellationToken cancellationToken)
//        {
//            var koi = await _koiRepository.FindAsync(x => x.ID == request.ID && x.DeletedTime == null, cancellationToken);
//            if (koi == null)
//            {
//                throw new NotFoundException("Koi not found");
//            }

//            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
//            if (user == null)
//            {
//                throw new NotFoundException("Please log in again");
//            }

//            var koiUpdate = _mapper.Map(request, koi);
//            if (koiUpdate != null)
//            {
//                koiUpdate.LastUpdatedBy = user.UserName;
//                koiUpdate.LastUpdatedTime = DateTimeOffset.Now;

//                _koiRepository.Update(koiUpdate);
//                await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
//                return "Koi updated successfully";
//            }

//            return "Error occurred while updating the KoiEntity";
//        }
//    }
//}
