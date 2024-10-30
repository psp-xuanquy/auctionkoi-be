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

        /// <summary>
        /// Gets all current koi requests for the authenticated koi breeder.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A list of current koi requests.</returns>
        /// <response code="200">Returns a list of current koi requests.</response>
        [HttpGet]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("kois/current")]
        public async Task<ActionResult<List<GetAllKoisRequestCurrentResponse>>> GetAllCurrentKoisRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllKoisRequestCurrentQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllKoisRequestCurrentResponse>>("Get all current Kois request successfully", result));
        }

        /// <summary>
        /// Gets all pending koi requests for managers and staff.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A list of pending koi requests.</returns>
        /// <response code="200">Returns a list of pending koi requests.</response>
        [HttpGet]
        [Authorize(Roles = "MANAGER, STAFF")]
        [Route("kois/pending")]
        public async Task<ActionResult<List<GetAllPendingKoisResponse>>> GetAllPendingKoisRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPendingKoisQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllPendingKoisResponse>>("Get all pending Kois request successfully", result));
        }

        /// <summary>
        /// Sends a koi auction request from a koi breeder.
        /// </summary>
        /// <param name="command">The command containing details of the koi auction request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the request.</returns>
        /// <response code="200">Returns success message if the request is sent successfully.</response>
        [HttpPost]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("koi")]
        public async Task<ActionResult<string>> SendKoiRequest([FromBody] SendRequestKoiAuctionCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Send Koi request successfully", result));
        }

        /// <summary>
        /// Resends a koi auction request from a koi breeder.
        /// </summary>
        /// <param name="command">The command containing details for resending the koi auction request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the resend request.</returns>
        /// <response code="200">Returns success message if the request is resent successfully.</response>
        [HttpPut]
        [Authorize(Roles = "KOIBREEDER")]
        [Route("koi/resend")]
        public async Task<ActionResult<string>> ResendKoiRequest([FromBody] ResendRequestKoiAuctionCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Resend Koi request successfully", result));
        }

        /// <summary>
        /// Approves a koi request by a manager or staff member.
        /// </summary>
        /// <param name="command">The command containing details for approving the koi request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the approval.</returns>
        /// <response code="200">Returns success message if the request is approved successfully.</response>
        [HttpPut]
        [Authorize(Roles = "MANAGER, STAFF")]
        [Route("koi/approval")]
        public async Task<ActionResult<string>> ApproveKoiRequest([FromBody] ApproveKoiRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Approve Koi request successfully", result));
        }

        /// <summary>
        /// Denies a koi request by a manager or staff member.
        /// </summary>
        /// <param name="command">The command containing details for denying the koi request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the denial.</returns>
        /// <response code="200">Returns success message if the request is denied successfully.</response>
        [HttpPut]
        [Authorize(Roles = "MANAGER, STAFF")]
        [Route("koi/denial")]
        public async Task<ActionResult<string>> DenyKoiRequest([FromBody] DenyKoiRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Deny Koi request successfully", result));
        }

        /// <summary>
        /// Gets all current role requests for the authenticated customer or koi breeder.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A list of current role requests.</returns>
        /// <response code="200">Returns a list of current role requests.</response>
        [HttpGet]
        [Authorize(Roles = "CUSTOMER, KOIBREEDER")]
        [Route("role/current")]
        public async Task<ActionResult<List<GetRequestCurrentUserResponse>>> GetAllCurrentRolesRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetRequestCurrentUserQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetRequestCurrentUserResponse>>("Get all curnet Roles request successfully", result));
        }

        /// <summary>
        /// Gets all pending role requests for managers.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A list of pending role requests.</returns>
        /// <response code="200">Returns a list of pending role requests.</response>
        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        [Route("role/pending")]
        public async Task<ActionResult<List<GetAllPendingRolesResponse>>> GetAllPendingRoleRequest(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllPendingRolesQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllPendingRolesResponse>>("Get all pending Roles request successfully", result));
        }

        /// <summary>
        /// Resends a role request from a customer.
        /// </summary>
        /// <param name="command">The command containing details for resending the role request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the resend request.</returns>
        /// <response code="200">Returns success message if the request is resent successfully.</response>
        [HttpPut]
        [Authorize(Roles = "CUSTOMER")]
        [Route("role/resend")]
        public async Task<ActionResult<string>> ResendRoleRequest([FromBody] ResendRegisterKoiBreederCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Resend register KoiBreeder successfully", result));
        }

        /// <summary>
        /// Approves a role request by a manager.
        /// </summary>
        /// <param name="command">The command containing details for approving the role request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the approval.</returns>
        /// <response code="200">Returns success message if the request is approved successfully.</response>
        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("role/approval")]
        public async Task<ActionResult<string>> ApproveRoleRequest([FromBody] ApproveRoleRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Approve Role request successfully", result));
        }

        /// <summary>
        /// Denies a role request by a manager.
        /// </summary>
        /// <param name="command">The command containing details for denying the role request.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>A response indicating the result of the denial.</returns>
        /// <response code="200">Returns success message if the request is denied successfully.</response>
        [HttpPut]
        [Authorize(Roles = "MANAGER")]
        [Route("role/denial")]
        public async Task<ActionResult<string>> DenyRoleRequest([FromBody] DenyRoleRequestCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>("Deny Role request successfully", result));
        }
    }
}
