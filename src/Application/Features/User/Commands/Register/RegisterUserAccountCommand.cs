﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.User.Commands.Register
{
    public class RegisterUserAccountCommand : IRequest<string>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }

    }
}