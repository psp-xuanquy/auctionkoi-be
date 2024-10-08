using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Email
{
    public class LoginUserAccountWithEmailResponse
    {
        public string? Email { get; set; }
        public string? ID { get; set; }
        public string? Role { get; set; }
    }
}
