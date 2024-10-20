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

        public AuthenticationController(UserManager<UserEntity> userManager, ISender mediator, IJwtService jwtService, IEmailService emailService)
        {
            _userManager = userManager;
            _mediator = mediator;
            _jwtService = jwtService;
            _emailService = emailService;

        }
 
        [HttpGet]
        [Route("accounts")]
        public async Task<ActionResult<List<GetUserAccountResponse>>> GetAll(
         CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllUserAccountQuery(), cancellationToken);
            return Ok(new JsonResponse<List<GetUserAccountResponse>>(result));
        }

        [HttpPost]
        [Route("register/customer")]
        public async Task<ActionResult<string>> CreateCustomerAccount([FromBody] RegisterCustomerAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new JsonResponse<string>(result));
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        [Route("register/koibreeder")]
        public async Task<ActionResult<string>> CreateKoiBreederAccount([FromBody] RegisterKoiBreederAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPost]
        [Route("register/manager")]
        public async Task<ActionResult<string>> CreateManagerAccount([FromBody] RegisterManagerAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPost]
        [Authorize(Roles = "MANAGER")]
        [Route("register/staff")]
        public async Task<ActionResult<string>> CreateStaffAccount([FromBody] RegisterStaffAccountCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(
                      [FromBody] LoginUserAccountWithEmailCommand query,
                                 CancellationToken cancellationToken = default)
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
