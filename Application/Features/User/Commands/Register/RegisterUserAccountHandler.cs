using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Application.User.Commands.Register
{
    public class RegisterUserAccountHandler : IRequestHandler<RegisterUserAccountCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public RegisterUserAccountHandler(IUserRepository userRepository,
                                          UserManager<AspNetUser> userManager,
                                          RoleManager<IdentityRole> roleManager,
                                          IEmailService emailService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<string> Handle(RegisterUserAccountCommand request, CancellationToken cancellationToken)
        {
            var accountExists = await _userRepository.FindAsync(_ => _.Email == request.Email && _.DeletedTime == null, cancellationToken);
            if (accountExists != null)
            {
                throw new DuplicationException("Email already exists");
            }

            var account = new AspNetUser
            {
                Email = request.Email,
                PasswordHash = _userRepository.HashPassword(request.Password),
                UserName = request.UserName,
                FullName = request.FullName,
                Gender = request.Gender,
                CreatedTime = DateTime.Now,
                LastUpdatedTime = DateTime.Now
            };

            // Add the new account to the database
            var createUserResult = await _userManager.CreateAsync(account);
            if (!createUserResult.Succeeded)
            {
                return "Account creation failed.";
            }

            // Check if the role "Member" exists
            var roleExists = await _roleManager.RoleExistsAsync("Member");
            if (!roleExists)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
                if (!createRoleResult.Succeeded)
                {
                    return "Failed to create Member role.";
                }
            }

            // Assign the "Member" role to the newly created account
            var addToRoleResult = await _userManager.AddToRoleAsync(account, "Member");
            if (!addToRoleResult.Succeeded)
            {
                return "Failed to assign Member role to the account.";
            }

            // Send confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);
            var confirmationLink = $"https://yourdomain.com/api/confirm-email?userId={account.Id}&token={Uri.EscapeDataString(token)}";
            await _emailService.SendConfirmEmailAsync(request.Email, request.UserName, confirmationLink);

            return "Account created successfully. Please check your email to verify your account.";
        }
    }
}
