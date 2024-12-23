﻿using KoiAuction.Application.Features.User.Commands.Login.Email;
using MediatR;

namespace Application.Features.Authentication.Commands.RegisterManager
{
    public class RegisterManagerAccountCommand : IRequest<LoginUserAccountWithEmailResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterManagerAccountCommand(string email, string password)
        {
            //Email = email;
            //Password = password;
            Email = "manager@manager.com";
            Password = "koimanager@123";
        }
    }
}
