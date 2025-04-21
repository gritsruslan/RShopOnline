using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;

namespace RShopAPI_Test.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IAuthenticationService authenticationService, 
        IIdentityProvider identityProvider)
    {
        string? userIdString = httpContext.User.Claims
            .FirstOrDefault(c => c.Type == JwtClaims.UserId)?.Value;

        if (Guid.TryParse(userIdString, out var userId))
        {
            var identity = await authenticationService.Authenticate(userId);
            identityProvider.Current = identity;
        }
        else
        {
            identityProvider.Current = UserIdentity.Guest;
        }
        
        await next(httpContext);
    }
}