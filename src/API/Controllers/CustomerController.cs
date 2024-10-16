using KoiAuction.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly UserManager<UserEntity> _userManager;

    public CustomerController(UserManager<UserEntity> userManager, ISender mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    

   

   

}
