using MediatR;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using KoiAuction.Domain.Entities;
using Domain.Enums;
using KoiAuction.Application.Features.User.Commands.Login.Email;

namespace Application.Features.Authentication.Commands.RegisterManager
{
    public class RegisterManagerAccountHandler : IRequestHandler<RegisterManagerAccountCommand, LoginUserAccountWithEmailResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterManagerAccountHandler(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<LoginUserAccountWithEmailResponse> Handle(RegisterManagerAccountCommand request, CancellationToken cancellationToken)
        {
            var accExist = await _userRepository.FindAsync(_ => _.Email == request.Email && _.DeletedTime == null, cancellationToken);
            if (accExist != null)
            {
                throw new DuplicationException("Email already exists");
            }

            var acc = new UserEntity
            {
                Email = request.Email,
                PasswordHash = _userRepository.HashPassword(request.Password),
                UserName = "Manager",
                CreatedTime = DateTime.Now,
                EmailConfirmed = true,
                CreatedBy = "System",
                LastUpdatedBy = "System"
            };

            var createManagerResult = await _userManager.CreateAsync(acc);
            if (!createManagerResult.Succeeded)
            {
                throw new Exception("Account creation failed.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(Role.MANAGER.ToString());
            if (!roleExist)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = Enum.GetName(typeof(Role), Role.MANAGER) });
                if (!createRoleResult.Succeeded)
                {
                    throw new Exception("Failed to create Manager role.");
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(acc, Role.MANAGER.ToString());
            if (!addToRoleResult.Succeeded)
            {
                throw new Exception("Failed to assign Manager role to the account.");
            }

            var role = await _userManager.GetRolesAsync(accExist);

            var response = new LoginUserAccountWithEmailResponse
            {
                Email = accExist.Email,
                ID = accExist.Id,
                Role = role.FirstOrDefault()
            };

            return response;

            //return "Manager account created successfully";
        }
    }
}
