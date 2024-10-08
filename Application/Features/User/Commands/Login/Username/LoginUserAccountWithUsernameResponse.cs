using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.Features.User.Commands.Login.Username
{
    public class LoginUserAccountWithUsernameResponse
    {
        public string? UserName { get; set; }
        public string? ID { get; set; }
        public string? Role { get; set; }
    }
}
