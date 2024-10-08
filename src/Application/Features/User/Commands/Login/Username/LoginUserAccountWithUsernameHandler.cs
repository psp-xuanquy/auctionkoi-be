using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Username
{
    public class LoginUserAccountWithUsernameHandler : IRequestHandler<LoginUserAccountWithUsernameCommand, LoginUserAccountWithUsernameResponse>
    {
        private readonly UserManager<AspNetUser> _userManager;  //dùng để tương tác với bảng AspNetUserRoles
        private readonly IUserRepository _userRepository;

        public LoginUserAccountWithUsernameHandler(UserManager<AspNetUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;

        }

        public async Task<LoginUserAccountWithUsernameResponse> Handle(LoginUserAccountWithUsernameCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                throw new NotFoundException("Username not found");
            }

            var password = _userRepository.VerifyPassword(request.Password, user.PasswordHash);
            if (!password)
            {
                throw new UnauthorizedAccessException("Wrong password, please try again");
            }

            var role = await _userManager.GetRolesAsync(user);

            var response = new LoginUserAccountWithUsernameResponse
            {
                UserName = user.UserName,
                ID = user.Id,
                Role = role.FirstOrDefault()
            };

            return response;
        }
    }
}
