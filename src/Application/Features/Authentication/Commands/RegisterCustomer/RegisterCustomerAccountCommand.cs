using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiAuction.Application.User.Commands.RegisterCustomer
{
    public class RegisterCustomerAccountCommand : IRequest<string>
    {    
        public string? Email { get; set; }
        public string? Password { get; set; }
        //public string? FullName { get; set; }
        //public string? UserName { get; set; }
        //public string? Address { get; set; }        
        //public string? PhoneNumber { get; set; }
        //public string? Gender { get; set; }
    }
}
