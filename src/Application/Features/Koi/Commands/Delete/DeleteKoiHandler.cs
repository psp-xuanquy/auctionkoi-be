using System.Threading;
using System.Threading.Tasks;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Commands.Delete
{
    public class DeleteKoiHandler : IRequestHandler<DeleteKoiCommand, string>
    {
        private readonly IKoiRepository _koiRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public DeleteKoiHandler(IKoiRepository koiRepository, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _koiRepository = koiRepository;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<string> Handle(DeleteKoiCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("Please login again");
            }

            var koi = await _koiRepository.FindAsync(x => x.ID == request.Id, cancellationToken);
            if (koi == null)
            {
                throw new NotFoundException("Koi not found.");
            }

            _koiRepository.Remove(koi);
            var result = await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return "Koi deleted successfully";
            }

            return "Failed to delete Koi";
        }
    }
}
