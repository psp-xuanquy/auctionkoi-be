﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Features.Authentication.Commands.RegisterStaff;
public class RegisterStaffAccountCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }

}