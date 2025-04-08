using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Interfaces;
using RShopAPI_Test.Services.Jwt;

namespace RShopAPI_Test.Services.Auth;

public class RoleAuthorizationHandler(IServiceScopeFactory scopeFactory) : AuthorizationHandler<RoleRequirements>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        RoleRequirements requirement)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == JwtClaims.UserId);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            return;
        }
        
        using var scope = scopeFactory.CreateScope();
        var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
        var userRole = await roleService.GetUserRoleById(userId, CancellationToken.None);

        if (userRole is null)
        {
            return;
        }

        if (!requirement.Roles.Contains((Role)userRole))
        {
            return;
        }
        
        context.Succeed(requirement);
    }
}