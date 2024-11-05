using Castle.Core.Resource;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using ExcelDataReader.Log;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Features.User.Commands.Login.Email;
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
    public class RegisterCustomerAccountHandler : IRequestHandler<RegisterCustomerAccountCommand, LoginUserAccountWithEmailResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly INotificationRepository _notificationRepository;

        public RegisterCustomerAccountHandler(IUserRepository userRepository,
                                          UserManager<UserEntity> userManager,
                                          RoleManager<IdentityRole> roleManager,
                                          INotificationRepository notificationRepository,
                                          IEmailService emailService)                                 
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;         
            _notificationRepository = notificationRepository;
        }

        public async Task<LoginUserAccountWithEmailResponse> Handle(RegisterCustomerAccountCommand request, CancellationToken cancellationToken)
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
                //FullName = request.FullName,
                UserName = request.Email,
                //PhoneNumber = request.PhoneNumber,
                //Address = request.Address,
                //Gender = request.Gender,
                Status = true,
                Balance = 1000,
                EmailConfirmed = true,
                CreatedTime = DateTime.Now,
                //CreatedBy = request.UserName,
                //LastUpdatedBy = request.UserName
            };

            // Add the new account to the database
            var createUserResult = await _userManager.CreateAsync(account);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                throw new Exception($"Account creation failed: {errors}");
            }

            // Check if the role "Customer" exists
            var roleExists = await _roleManager.RoleExistsAsync(Role.CUSTOMER.ToString());
            if (!roleExists)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = Enum.GetName(typeof(Role), Role.CUSTOMER) });
                if (!createRoleResult.Succeeded)
                {
                    throw new Exception("Failed to create Customer role.");
                }
            }

            // Assign the "Customer" role to the newly created account
            var addToRoleResult = await _userManager.AddToRoleAsync(account, Role.CUSTOMER.ToString());
            if (!addToRoleResult.Succeeded)
            {
                throw new Exception("Failed to assign Customer role to the account.");
            }

            await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var role = await _userManager.GetRolesAsync(account);

            var response = new LoginUserAccountWithEmailResponse
            {
                Email = account.Email,
                ID = account.Id,
                Role = role.FirstOrDefault()
            };

            var notification = new NotificationEntity
            {
                UserID = account.Id,
                Message = "Welcome to the most reputable Koi fish auction site in Vietnam. For any inquiries, please contact the Administrator for assistance.",
                MarkAsRead = false,
                CreatedTime = DateTime.UtcNow,
                CreatedBy = "System"  
            };
            _notificationRepository.Add(notification);
            await _notificationRepository.UnitOfWork.SaveChangesAsync();

            return response;

           // Send confirmation email
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(account);
            //var confirmationLink = $"https://koiauctionwebapp.azurewebsites.net/ConfirmEmail/confirm-email?userId={account.Id}&token={Uri.EscapeDataString(token)}";
            //await _emailService.SendConfirmEmailAsync(request.Email, confirmationLink);

            //return "Account created successfully. Please check your email to verify your account.";
        }
    }
}
