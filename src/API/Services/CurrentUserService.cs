using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using KoiAuction.Application.Common.Interfaces;
using System.Security.Claims;

namespace KoiAuction.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal? _claimsPrincipal;
        private readonly IAuthorizationService _authorizationService;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
        {
            _claimsPrincipal = httpContextAccessor?.HttpContext?.User;
            _authorizationService = authorizationService;
        }

        public string? UserId => _claimsPrincipal?.FindFirst(JwtClaimTypes.Subject)?.Value;

        public async Task<bool> AuthorizeAsync(string policy)
        {
            if (_claimsPrincipal == null) return false;
            return (await _authorizationService.AuthorizeAsync(_claimsPrincipal, policy)).Succeeded;
        }

        public async Task<bool> IsInRoleAsync(string role)
        {
            return await Task.FromResult(_claimsPrincipal?.IsInRole(role) ?? false);
        }

        //public string GetCurrentUserId()
        //{
        //    if (_claimsPrincipal == null)
        //    {
        //        throw new Exception("Http context is null. Please Login.");
        //    }

        //    if (!_claimsPrincipal.Identity.IsAuthenticated)
        //    {
        //        throw new UnauthorizedAccessException("User is not Authenticated");
        //    }

        //    var currentUserId = _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(currentUserId))
        //    {
        //        throw new UnauthorizedAccessException("User ID claim is not found");
        //    }

        //    return currentUserId;
        //}
    }
}
