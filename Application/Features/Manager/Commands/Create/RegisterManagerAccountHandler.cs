using MediatR;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.IRepositories;
using KoiAuction.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Identity;
using KoiAuction.Domain.Entities;

namespace KoiAuction.Application.Admin.Commands.Create
{
    public class RegisterManagerAccountHandler : IRequestHandler<RegisterManagerAccountCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterManagerAccountHandler(IUserRepository userRepository, UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Handle(RegisterManagerAccountCommand request, CancellationToken cancellationToken)
        {
            var accExist = await _userRepository.FindAsync(_ => _.Email == request.Email && _.DeletedTime == null, cancellationToken);
            if (accExist != null)
            {
                throw new DuplicationException("Email already exists");
            }

            var acc = new AspNetUser
            {
                Email = request.Email,
                PasswordHash = _userRepository.HashPassword(request.Password),
                UserName = "Manager_Koi",
                CreatedTime = DateTime.Now,
                CreatedBy = "System",
                LastUpdatedBy = "System"
            };

            var createManagerResult = await _userManager.CreateAsync(acc);
            if (!createManagerResult.Succeeded)
            {
                return "Account creation failed.";
            }

            var roleExist = await _roleManager.RoleExistsAsync("Manager");
            if (!roleExist)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = "Manager" });
                if (!createRoleResult.Succeeded)
                {
                    return "Failed to create Manager role.";
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(acc, "Manager");
            if (!addToRoleResult.Succeeded)
            {
                return "Failed to assign Manager role to the account.";
            }

            return "Manager account created successfully";
        }
    }
}
