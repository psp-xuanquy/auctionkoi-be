//using KoiAuction.Application.Common.Interfaces;
//using KoiAuction.Domain.Common.Exceptions;
//using KoiAuction.Domain.IRepositories;
//using MediatR;

//namespace KoiAuction.Application.Features.Koi.Commands.Delete
//{
//    public class DeleteKoiHandler : IRequestHandler<DeleteKoiCommand, string>
//    {
//        private readonly IKoiRepository _koiRepository;
//        private readonly ICurrentUserService _currentUserService;
//        private readonly IUserRepository _userRepository;

//        public DeleteKoiHandler(IKoiRepository koiRepository, ICurrentUserService currentUserService, IUserRepository userRepository)
//        {
//            _koiRepository = koiRepository;
//            _currentUserService = currentUserService;
//            _userRepository = userRepository;
//        }

//        public async Task<string> Handle(DeleteKoiCommand request, CancellationToken cancellationToken)
//        {

//            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
//            if (user == null)
//            {
//                throw new NotFoundException("Please log in again");
//            }

//            var koi = await _koiRepository.FindAsync(x => x.ID == request.ID && x.DeletedBy == null, cancellationToken);
//            if (koi == null)
//            {
//                throw new NotFoundException("Koi not found");
//            }

//            koi.DeletedBy = user.UserName;
//            koi.DeletedTime = DateTimeOffset.Now;

//            _koiRepository.Update(koi);

//            if (await _koiRepository.UnitOfWork.SaveChangesAsync() > 0)
//                return "Koi deleted successfully";
//            else
//                return "Koi deleted successfully";
//        }
//    }
//}
