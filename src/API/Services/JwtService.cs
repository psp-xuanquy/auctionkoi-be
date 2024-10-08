using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using KoiAuction.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KoiAuction.API.Services
{
    public class JwtService : IJwtService
    {
        public string CreateToken(string ID, string roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, ID),   //Sub là mã định danh
                new(ClaimTypes.Role, roles)             //Vai trò người dùng
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is KN Spices Website SecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                // issuer: "test",
                // audience: "api",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}