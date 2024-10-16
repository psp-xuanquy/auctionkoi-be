using AutoMapper;
using Domain.Enums;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
public class ResendRegisterKoiBreederHandler : IRequestHandler<ResendRegisterKoiBreederCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IKoiBreederRepository _koiBreederRepository;
    private readonly IMapper _mapper;

    public ResendRegisterKoiBreederHandler(IUserRepository userRepository,
                                      UserManager<UserEntity> userManager,
                                      RoleManager<IdentityRole> roleManager,
                                      IMapper mapper,
                                      ICurrentUserService currentUserService,
                                      IKoiBreederRepository koiBreederRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _koiBreederRepository = koiBreederRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<string> Handle(ResendRegisterKoiBreederCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var existingRequest = await _koiBreederRepository.FindAsync(x => x.ID == request.KoiBreederID && x.UserId == _currentUserService.UserId && x.RoleRequestStatus != RoleRequestStatus.Approved);
        if (existingRequest == null)
        {
            throw new DuplicationException("Your request does not exist or has already been approved.");
        }

        // Map các thuộc tính từ request sang existingRequest
        var koibreederUpdate = _mapper.Map(request, existingRequest);
        if (koibreederUpdate != null)
        {
            // Cập nhật trạng thái yêu cầu
            koibreederUpdate.RoleRequestStatus = RoleRequestStatus.Pending;
            _koiBreederRepository.Update(koibreederUpdate);
            await _koiBreederRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return "Resend request success. Please wait to be approved.";
        }

        return "Failed to resend request.";
    }
}
