using Application.Features.Request.Customer.Commands.ResendRegisterKoiBreeder;
using Application.Features.Request.KoiBreeder.Commands.ResendRequestKoiAuction;
using Application.Features.Request.KoiBreeder.Commands.SendRequestKoiAuction;
using Application.Features.Request.KoiBreeder.Queries.GetAllKoiRequest;
using Application.Features.Request.Manager.Commands.ApproveKoiRequest;
using Application.Features.Request.Manager.Commands.ApproveRoleRequest;
using Application.Features.Request.Manager.Commands.DenyKoiRequest;
using Application.Features.Request.Manager.Commands.DenyRoleRequest;
using Application.Features.Request.Manager.Queries.GetAllPendingKois;
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

        [HttpGet]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("kois/current")]
        public async Task<ActionResult<List<GetAllKoisRequestCurrentResponse>>> GetAllCurrentKoisRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoisRequestCurrentQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllKoisRequestCurrentResponse>>(result));
        }

        [HttpGet]
        [Authorize(Roles = "MANAGER, STAFF")]
        [Route("kois/pending")]
        public async Task<ActionResult<List<GetAllPendingKoisResponse>>> GetAllPendingKoisRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPendingKoisQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllPendingKoisResponse>>(result));
        }

        [HttpPost]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("koi")]
        public async Task<ActionResult<string>> SendKoiRequest([FromBody] SendRequestKoiAuctionCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPut]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("koi/resend")]
        public async Task<ActionResult<string>> ResendKoiRequest([FromBody] ResendRequestKoiAuctionCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
       
        [HttpPut]
        [Authorize(Roles = "MANAGER, STAFF")]
        [Route("koi/approval")]
        public async Task<ActionResult<string>> ApproveKoiRequest([FromBody] ApproveKoiRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }
       
        [HttpPut]
        [Authorize(Roles = "MANAGER, STAFF")]
        [Route("koi/denial")]
        public async Task<ActionResult<string>> DenyKoiRequest([FromBody] DenyKoiRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        // Customers can retrieve their own sent requests
        [HttpGet]
        [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
        [Route("role/current")]
        public async Task<ActionResult<List<GetRequestCurrentUserResponse>>> GetAllCurrentRolesRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetRequestCurrentUserQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetRequestCurrentUserResponse>>(result));
        }

        // Managers can get all pending requests
        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        [Route("role/pending")]
        public async Task<ActionResult<List<GetAllPendingRolesResponse>>> GetAllPendingRoleRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPendingRolesQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllPendingRolesResponse>>(result));
        }

        // Customers can review request response, edit their requests, and resend them
        [HttpPut]
        [Authorize(Roles = "CUSTOMER")]
        [Route("role/resend")]
        public async Task<ActionResult<string>> ResendRoleRequest([FromBody] ResendRegisterKoiBreederCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        // Managers can approve requests
        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("role/approval")]
        public async Task<ActionResult<string>> ApproveRoleRequest([FromBody] ApproveRoleRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        // Managers can deny requests
        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("role/denial")]
        public async Task<ActionResult<string>> DenyRoleRequest([FromBody] DenyRoleRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

    }
}
