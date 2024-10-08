using KoiAuction.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Email
{
    public class LoginUserAccountWithEmailCommand : IRequest<LoginUserAccountWithEmailResponse>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
