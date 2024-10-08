using MediatR;

namespace KoiAuction.Application.Admin.Commands.Create
{
    public class RegisterManagerAccountCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterManagerAccountCommand(string email, string password)
        {
            //Email = email;
            //Password = password;
            Email = "koimanager@gmail.com";
            Password = "koimanager@123";
        }
    }
}
