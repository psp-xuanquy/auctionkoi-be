using Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
using Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
using Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
using Application.Features.Request.Manager.Commands.ApproveRoleRequest;
using Application.Features.Request.Manager.Commands.DenyRoleRequest;
using Application.Features.Request.Manager.Queries.GetAllPendingRoles;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly UserManager<UserEntity> _userManager;

        public RequestController(UserManager<UserEntity> userManager, ISender mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        // Customers can retrieve their own sent requests
        [HttpGet]
        [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
        [Route("user/request")]
        public async Task<IActionResult> GetUserRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetRequestCurrentUserQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetRequestCurrentUserResponse>>(result));
        }

        // Customers can review request response, edit their requests, and resend them
        [HttpPut]
        [Authorize(Roles = "CUSTOMER")]
        [Route("customer/resend")]
        public async Task<IActionResult> ResendRoleRequest([FromBody] ResendRegisterKoiBreederCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpGet]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("koibreeder/kois")]
        public async Task<IActionResult> GetKoisRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoiRequestQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllKoiRequestResponse>>(result));
        }

        [HttpPost]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("koibreeder/koi")]
        public async Task<IActionResult> SendKoiRequest([FromBody] SendRequestKoiAuctionCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }


        // Managers can get all pending requests
        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        [Route("manager/requests")]
        public async Task<IActionResult> GetAllPendingRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPendingRolesQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllPendingRolesResponse>>(result));
        }

        // Managers can approve requests
        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("manager/approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveRoleRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        // Managers can deny requests
        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("manager/deny")]
        public async Task<IActionResult> Deny([FromBody] DenyRoleRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }


    }
}
