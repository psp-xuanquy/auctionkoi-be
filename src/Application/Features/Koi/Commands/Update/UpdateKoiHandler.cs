using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;

namespace Application.Features.Koi.Commands.Update;
public class UpdateKoiHandler : IRequestHandler<UpdateKoiCommand, string>
{
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IKoiRepository _koiRepository;
    public UpdateKoiHandler(IMapper mapper, ICurrentUserService currentUserService, IUserRepository userRepository, IKoiRepository koiRepository)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _koiRepository = koiRepository;
    }

    public async Task<string> Handle(UpdateKoiCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var Koi = await _koiRepository.FindAsync(x => x.ID == request.Id);
        if (Koi == null)
        {
            throw new NotFoundException("Koi not found");
        }

        var update = _mapper.Map(request, Koi);
        if (update != null)
        {
            _koiRepository.Update(update);
            await _koiRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return "Update Koi successfully";
        }

        return "Error occurred while updating Koi";
    }
}
