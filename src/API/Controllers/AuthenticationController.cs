using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using KoiAuction.Application.User.Queries.GetAll;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Application.Features.User.Commands.Login.Email;
using Microsoft.AspNetCore.Identity;
using KoiAuction.Domain.Entities;
using KoiAuction.Application.User.Commands.RegisterCustomer;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Authentication.Commands.RegisterManager;
using Application.Features.Authentication.Commands.RegisterKoiBreeder;
using Application.Features.Authentication.Queries.GetAll;
using Application.Features.Request.User.Queries.GetRequestCurrentUser;
using Application.Features.Authentication.Commands.RegisterStaff;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
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
        /// <param name="emailService">The email service for sending confirmation and reset emails.</param>
        public AuthenticationController(UserManager<UserEntity> userManager, ISender mediator, IJwtService jwtService, IEmailService emailService)
        {
            _userManager = userManager;
            _mediator = mediator;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        /// <summary>
        /// Retrieves all user accounts.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A list of user accounts.</returns>
        /// <response code="200">Returns the list of user accounts.</response>
        [HttpGet("accounts")]
        public async Task<ActionResult<List<GetUserAccountResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllUserAccountQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetUserAccountResponse>>(result));
        }

        /// <summary>
        /// Creates a new customer account.
        /// </summary>
        /// <param name="command">The command containing the details of the customer account to create.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The ID of the created customer account.</returns>
        /// <response code="201">Returns the ID of the created customer account.</response>
        [HttpPost("register/customer")]
        public async Task<ActionResult<string>> CreateCustomerAccount([FromBody] RegisterCustomerAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new JsonResponse<string>(result));
        }

        /// <summary>
        /// Creates a new koi breeder account.
        /// </summary>
        /// <param name="command">The command containing the details of the koi breeder account to create.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The ID of the created koi breeder account.</returns>
        /// <response code="200">Returns the ID of the created koi breeder account.</response>
        [HttpPost("register/koibreeder")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ActionResult<string>> CreateKoiBreederAccount([FromBody] RegisterKoiBreederAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// Creates a new manager account.
        /// </summary>
        /// <param name="command">The command containing the details of the manager account to create.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The ID of the created manager account.</returns>
        /// <response code="200">Returns the ID of the created manager account.</response>
        [HttpPost("register/manager")]
        public async Task<ActionResult<string>> CreateManagerAccount([FromBody] RegisterManagerAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// Creates a new staff account.
        /// </summary>
        /// <param name="command">The command containing the details of the staff account to create.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>The ID of the created staff account.</returns>
        /// <response code="200">Returns the ID of the created staff account.</response>
        [HttpPost("register/staff")]
        [Authorize(Roles = "MANAGER")]
        public async Task<ActionResult<string>> CreateStaffAccount([FromBody] RegisterStaffAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        /// <param name="query">The command containing login details.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A JWT token for the logged-in user.</returns>
        /// <response code="200">Returns the JWT token.</response>
        /// <response code="400">If the login details are invalid.</response>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserAccountWithEmailCommand query, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            var token = _jwtService.CreateToken(result.ID, result.Role);
            return Ok(new JsonResponse<string>(token));
        }

        //[HttpPost]
        //[Route("resend-confirmation")]
        //[Produces(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> ResendConfirmationEmail(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null || user.EmailConfirmed)
        //    {
        //        return BadRequest(new JsonResponse<string>("Người dùng không tồn tại hoặc Email đã được xác thực."));
        //    }

        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    var confirmationLink = $"https://yourdomain.com/api/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        //    await _emailService.SendConfirmEmailAsync(user.Email, user.UserName, confirmationLink);
        //    return Ok(new JsonResponse<string>("Mail xác thực được gửi lại thành công."));
        //}

        //[HttpPost]
        //[Route("forgot-password")]
        //[Produces(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> ForgotPassword(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null || !user.EmailConfirmed)
        //    {
        //        return BadRequest(new JsonResponse<string>("Người dùng không tồn tại hoặc Email chưa được xác thực."));
        //    }

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var resetLink = $"https://yourdomain.com/api/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        //    await _emailService.SendResetPasswordEmailAsync(user.Email, resetLink);
        //    return Ok(new JsonResponse<string>("Email đặt lại mật khẩu đã được gửi."));
        //}

        //[HttpPost]
        //[Route("reset-password")]
        //[Produces(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);
        //    if (user == null) return BadRequest(new JsonResponse<string>("Người dùng không tồn tại."));

        //    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        return Ok(new JsonResponse<string>("Đặt lại mật khẩu thành công."));
        //    }

        //    return BadRequest(new JsonResponse<string>("Có lỗi xảy ra trong quá trình đặt lại mật khẩu."));
        //}
    }
}
