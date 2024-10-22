using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using KoiAuction.Application.Common.Interfaces;
using KoiAuction.Domain.Entities;

namespace KoiAuction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConfirmEmailController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmEmailController"/> class.
        /// </summary>
        /// <param name="userManager">The UserManager instance used for managing user accounts.</param>
        public ConfirmEmailController(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Confirms the user's email using the provided user ID and token.
        /// </summary>
        /// <param name="userId">The ID of the user whose email is to be confirmed.</param>
        /// <param name="token">The token for email confirmation.</param>
        /// <returns>An IActionResult indicating the result of the email confirmation.</returns>
        /// <response code="200">Returns success message if the email confirmation is successful.</response>
        /// <response code="400">Returns error message if the user is not found or the confirmation fails.</response>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                // Đăng nhập người dùng sau khi xác nhận email
                // await SignInUser(user);
                return Ok("Confirm Email Success! Now you can Login");
                // return Redirect("http://localhost:5154/swagger/index.html"); // Redirect đến trang chính sau khi đăng nhập thành công
            }

            return BadRequest("Email confirms failed.");
        }

        /// <summary>
        /// Signs in the user with the provided claims.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        private async Task SignInUser(UserEntity user)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = await _userManager.GetClaimsAsync(user);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
