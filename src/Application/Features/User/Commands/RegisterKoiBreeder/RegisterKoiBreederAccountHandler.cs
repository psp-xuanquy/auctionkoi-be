using AutoMapper;
using Castle.Core.Resource;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Application.User.Commands.RegisterKoiBreeder
{
    public class RegisterKoiBreederAccountHandler : IRequestHandler<RegisterKoiBreederAccountCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IKoiBreederRepository _koiBreederRepository;
        private readonly IMapper _mapper;

        public RegisterKoiBreederAccountHandler(IUserRepository userRepository,
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

        public async Task<string> Handle(RegisterKoiBreederAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
            if (user == null)
            {
                throw new NotFoundException("Please login again");
            }

            var existingRequest = await _koiBreederRepository.FindAsync(x => x.UserId == _currentUserService.UserId);
            if (existingRequest != null)
            {
                throw new DuplicationException("Your request has been sent.");
            }

            var koibreeder = _mapper.Map<KoiBreederEntity>(request);
            koibreeder.UserId = _currentUserService.UserId;
            koibreeder.RoleRequestStatus = RoleRequestStatus.Pending;

            _koiBreederRepository.Add(koibreeder);
            await _koiBreederRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return "Register to become Koi Breeder success. Please wait to be approved.";

        }
    }
}


