using KoiAuction.API.Controllers.ResponseTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using KoiAuction.Application.User.Queries.GetAll;
using KoiAuction.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using KoiAuction.Application.User.Queries;
using System.Net.Mime;
using KoiAuction.Application.Features.User.Commands.Login.Email;
using KoiAuction.Application.Features.User.Commands.Login.Username;
using Microsoft.AspNetCore.Identity;
using KoiAuction.Application.Common.Password;
using KoiAuction.Domain.Entities;
using KoiAuction.Application.User.Commands.Register;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly UserManager<AspNetUser> _userManager;

        public UserController(UserManager<AspNetUser> userManager, ISender mediator, IJwtService jwtService, IEmailService emailService)
        {
            _userManager = userManager;
            _mediator = mediator;
            _jwtService = jwtService;
            _emailService = emailService;

        }

        [HttpGet]
        [Authorize]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<List<GetUserAccountResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GetUserAccountResponse>>> GetAll(
          CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllUserAccountQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetUserAccountResponse>>(result));
        }

        [HttpPost]
        [Route("register")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUserAccount([FromBody] RegisterUserAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new JsonResponse<string>(result));
        }

        [HttpPost]
        [Route("login-by-username")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Login(
                      [FromBody] LoginUserAccountWithUsernameCommand query,
                                 CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            var token = _jwtService.CreateToken(result.ID, result.Role);
            return Ok(new JsonResponse<string>(token));
        }

        [HttpPost]
        [Route("login-by-email")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> Login(
                      [FromBody] LoginUserAccountWithEmailCommand query,
                                 CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            var token = _jwtService.CreateToken(result.ID, result.Role);
            return Ok(new JsonResponse<string>(token));
        }

        [HttpPost]
        [Route("resend-confirmation")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.EmailConfirmed)
            {
                return BadRequest(new JsonResponse<string>("Người dùng không tồn tại hoặc Email đã được xác thực."));
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://yourdomain.com/api/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendConfirmEmailAsync(user.Email, user.UserName, confirmationLink);
            return Ok(new JsonResponse<string>("Mail xác thực được gửi lại thành công."));
        }

        [HttpPost]
        [Route("forgot-password")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
            {
                return BadRequest(new JsonResponse<string>("Người dùng không tồn tại hoặc Email chưa được xác thực."));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://yourdomain.com/api/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendResetPasswordEmailAsync(user.Email, resetLink);
            return Ok(new JsonResponse<string>("Email đặt lại mật khẩu đã được gửi."));
        }

        [HttpPost]
        [Route("reset-password")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return BadRequest(new JsonResponse<string>("Người dùng không tồn tại."));

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new JsonResponse<string>("Đặt lại mật khẩu thành công."));
            }

            return BadRequest(new JsonResponse<string>("Có lỗi xảy ra trong quá trình đặt lại mật khẩu."));
        }
    }
}