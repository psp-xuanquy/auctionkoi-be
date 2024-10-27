using Castle.Core.Resource;
using Domain.Enums;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KoiAuction.Application.User.Commands.RegisterCustomer
{
    public class RegisterCustomerAccountHandler : IRequestHandler<RegisterCustomerAccountCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
       

        public RegisterCustomerAccountHandler(IUserRepository userRepository,
                                          UserManager<UserEntity> userManager,
                                          RoleManager<IdentityRole> roleManager,
                                          IEmailService emailService)                                 
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;         
        }

        public async Task<string> Handle(RegisterCustomerAccountCommand request, CancellationToken cancellationToken)
        {
            var accountExists = await _userRepository.FindAsync(_ => _.Email == request.Email, cancellationToken);
            if (accountExists != null)
            {
                throw new DuplicationException("Email already exists");
            }

            var account = new UserEntity
            {
                Email = request.Email,
                PasswordHash = _userRepository.HashPassword(request.Password),
                FullName = request.FullName,
                UserName = request.UserName,             
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Gender = request.Gender,
                Status = true,
                Balance = 1000,
                EmailConfirmed = true,
                CreatedTime = DateTime.Now,
                CreatedBy = request.UserName,
                LastUpdatedBy = request.UserName
            };

            // Add the new account to the database
            var createUserResult = await _userManager.CreateAsync(account);
            if (!createUserResult.Succeeded)
            {
                return "Account creation failed.";
            }

            // Check if the role "Customer" exists
            var roleExists = await _roleManager.RoleExistsAsync(Role.CUSTOMER.ToString());
            if (!roleExists)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = Enum.GetName(typeof(Role), Role.CUSTOMER) });
                if (!createRoleResult.Succeeded)
                {
                    return "Failed to create Customer role.";
                }
            }

            // Assign the "Customer" role to the newly created account
            var addToRoleResult = await _userManager.AddToRoleAsync(account, Role.CUSTOMER.ToString());
            if (!addToRoleResult.Succeeded)
            {
                return "Failed to assign Customer role to the account.";
            }

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            // Send confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);
            var confirmationLink = $"https://koiauctionwebapp.azurewebsites.net/ConfirmEmail/confirm-email?userId={account.Id}&token={Uri.EscapeDataString(token)}";
            await _emailService.SendConfirmEmailAsync(request.Email, request.UserName, confirmationLink);

            return "Account created successfully. Please check your email to verify your account.";
        }
    }
}
