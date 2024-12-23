﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Authentication.Commands.RegisterManager;
using Domain.Enums;
using KoiAuction.Application.Features.User.Commands.Login.Email;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.RegisterStaff;
public class RegisterStaffAccountHandler : IRequestHandler<RegisterStaffAccountCommand, LoginUserAccountWithEmailResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterStaffAccountHandler(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<LoginUserAccountWithEmailResponse> Handle(RegisterStaffAccountCommand request, CancellationToken cancellationToken)
    {
        var accExist = await _userRepository.FindAsync(_ => _.Email == request.Email && _.DeletedTime == null, cancellationToken);
        if (accExist != null)
        {
            throw new DuplicationException("Email already exists");
        }

        var account = new UserEntity
        {
            Email = request.Email,
            PasswordHash = _userRepository.HashPassword(request.Password),
            //FullName = request.FullName,
            UserName = request.Email,
            //PhoneNumber = request.PhoneNumber,
            //Address = request.Address,
            //Gender = request.Gender,
            Status = true,
            //EmailConfirmed = true,
            CreatedTime = DateTime.Now,
            CreatedBy = "Manager",
            LastUpdatedBy = "Manager"
        };

        
        var createUserResult = await _userManager.CreateAsync(account);
        if (!createUserResult.Succeeded)
        {
            throw new Exception("Account creation failed.");
        }

        var roleExists = await _roleManager.RoleExistsAsync(Role.STAFF.ToString());
        if (!roleExists)
        {
            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = Enum.GetName(typeof(Role), Role.STAFF) });
            if (!createRoleResult.Succeeded)
            {
                throw new Exception("Failed to create Customer role.");
            }
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(account, Role.STAFF.ToString());
        if (!addToRoleResult.Succeeded)
        {
            throw new Exception("Failed to assign Customer role to the account.");
        }

        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var role = await _userManager.GetRolesAsync(account);

        var response = new LoginUserAccountWithEmailResponse
        {
            Email = account.Email,
            ID = account.Id,
            Role = role.FirstOrDefault()
        };

        return response;

        //return "Staff account created successfully";
    }
}
