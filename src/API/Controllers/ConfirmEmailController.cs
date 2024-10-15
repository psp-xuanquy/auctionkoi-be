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

        public ConfirmEmailController(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

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
            
                return Ok("Confirm Email Success! Now you can Login");
                //await SignInUser(user);
                //return Redirect("http://localhost:5154/swagger/index.html"); // Redirect đến trang chính sau khi đăng nhập thành công
            }

            return BadRequest("Email confirms failed.");
        }

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
