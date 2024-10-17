using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.IRepositories;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Common.Exceptions;
using KoiAuction.Domain.Entities;
using KoiAuction.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Request.Manager.Commands.ApproveRoleRequest;
public class ApproveRoleRequestHandler : IRequestHandler<ApproveRoleRequestCommand, string>
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IKoiBreederRepository _koiBreederRepository;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApproveRoleRequestHandler(UserManager<UserEntity> userManager, ICurrentUserService currentUserService, IUserRepository userRepository, IKoiBreederRepository koiBreederRepository, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _koiBreederRepository = koiBreederRepository;
        _roleManager = roleManager;
    }

    public async Task<string> Handle(ApproveRoleRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(x => x.Id == _currentUserService.UserId);
        if (user == null)
        {
            throw new NotFoundException("Please login again");
        }

        var koibreeder = await _koiBreederRepository.FindAsync(x => x.ID == request.KoiBreederID && x.RoleRequestStatus == RoleRequestStatus.Pending);
        if (koibreeder == null)
        {
            throw new NotFoundException("Request not found or has already been approved/denied.");
        }

        // Check if the role "KoiBreeder" exists
        var roleExists = await _roleManager.RoleExistsAsync(Role.KOIBREEDER.ToString());
        if (!roleExists)
        {
            var createRoleResult = await _roleManager.CreateAsync(new IdentityRole { Name = Enum.GetName(typeof(Role), Role.KOIBREEDER) });
            if (!createRoleResult.Succeeded)
            {
                return "Failed to create KoiBreeder role.";
            }
        }

        // Remove the old role "Customer"
        var removeCustomerRoleResult = await _userManager.RemoveFromRoleAsync(koibreeder.User, Role.CUSTOMER.ToString());
        if (!removeCustomerRoleResult.Succeeded)
        {
            return "Failed to remove the Customer role.";
        }

        // Assign the new role "KoiBreeder"
        var addBreederRoleResult = await _userManager.AddToRoleAsync(koibreeder.User, Role.KOIBREEDER.ToString());
        if (!addBreederRoleResult.Succeeded)
        {
            return "Failed to assign the KoiBreeder role.";
        }

        koibreeder.RoleRequestStatus = RoleRequestStatus.Approved;
        koibreeder.RequestResponse = "Your request has been approved";
        _koiBreederRepository.Update(koibreeder);
        await _koiBreederRepository.UnitOfWork.SaveChangesAsync();

        return $"You have approved the account {koibreeder.User.UserName} to become a new Koi Breeder with the name {koibreeder.KoiFarmName}.";
    }
}
