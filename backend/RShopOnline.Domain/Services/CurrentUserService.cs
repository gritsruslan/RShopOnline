using Microsoft.AspNetCore.Http;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;

namespace RShopAPI_Test.Services.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? GetCurrentUserId()
    {
        var userIdClaim = 
            httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}