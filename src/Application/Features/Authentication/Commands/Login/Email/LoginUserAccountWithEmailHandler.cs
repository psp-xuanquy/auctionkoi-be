using KoiAuction.Domain.Entities;
using KoiAuction.Domain.Repositories;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Email
{
    public class LoginUserAccountWithEmailHandler : IRequestHandler<LoginUserAccountWithEmailCommand, LoginUserAccountWithEmailResponse>
    {
        private readonly UserManager<UserEntity> _userManager;  //dùng để tương tác với bảng AspNetUserRoles
        private readonly IUserRepository _userRepository;

        public LoginUserAccountWithEmailHandler(UserManager<UserEntity> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;

        }

        public async Task<LoginUserAccountWithEmailResponse> Handle(LoginUserAccountWithEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException("Email not found");
            }

            var password = _userRepository.VerifyPassword(request.Password, user.PasswordHash);
            if (!password)
            {
                throw new UnauthorizedException("Wrong password, please try again");
            }

            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedException("Email not confirmed. Please confirm your email before logging in.");
            }

            var role = await _userManager.GetRolesAsync(user);

            var response = new LoginUserAccountWithEmailResponse
            {
                Email = user.Email,
                ID = user.Id,
                Role = role.FirstOrDefault()
            };

            return response;
        }
    }
}
