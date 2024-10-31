using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.API.Controllers.ResponseTypes;
using KoiAuction.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Features.User.CurrentUser.Queries.GetCurrentUser;
using Application.Features.User;
using System.Security.Claims;
using Application.Features.User.CurrentUser.Commands.Update;
using Application.Features.User.Manager.Queries.GetAllCurrentUsers;
using Application.Features.AuctionMethod.Commands.Update;
using Application.Features.User.Manager.Commands.Update;
using Application.Features.AuctionMethod.Commands.Delete;
using Application.Features.AuctionMethod;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly UserManager<UserEntity> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="userManager">The UserManager instance used for managing user accounts.</param>
        /// <param name="mediator">The mediator instance used for handling commands and queries.</param>
        /// <param name="jwtService">The JWT service for token creation.</param>
        public UserController(UserManager<UserEntity> userManager, ISender mediator, IJwtService jwtService)
        {
            _userManager = userManager;
            _mediator = mediator;
            _jwtService = jwtService;
        }

        [HttpGet("get-current-user")]
        public async Task<ActionResult<JsonResponse<GetCurrentUserResponse>>> GetLoggedUser(CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new GetCurrentUserQuery(userId), cancellationToken);
            return Ok(new JsonResponse<GetCurrentUserResponse>("Get Current User successfully.", result));
        }

        [HttpGet("get-all-current-user-by-manager")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<List<GetAllCurrentUsersResponse>>>> GetAllCurrentUser(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllCurrentUsersQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetAllCurrentUsersResponse>>("Get all Current User successfully.", result));
        }

        [HttpPut("update-current-user")]
        [Authorize]
        public async Task<ActionResult<JsonResponse<UserResponse>>> UpdateUser(UpdateCurrentUserCommand command, CancellationToken cancellationToken = default)
        {
            // Set UserId from claims
            command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<UserResponse>("User updated successfully.", result));
        }

        [HttpPut("update-user-by-manager/{userId}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<UserResponse>>> UpdateUserByManager(string userId, [FromBody] UpdateUserByManagerCommand command, CancellationToken cancellationToken = default)
        {
            var request = new UpdateUserByManagerRequest(userId, command);
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(new JsonResponse<UserResponse>("User updated successfully.", result));
        }

        [HttpDelete("delete-user-by-manager/{userId}")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<JsonResponse<string>>> DeleteUserByManager(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new DeleteUserByManagerCommand(userId: userId), cancellationToken);
            return Ok(new JsonResponse<string>(result, null));
        }
    }
}
