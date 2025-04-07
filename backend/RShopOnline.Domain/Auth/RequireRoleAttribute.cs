using Microsoft.AspNetCore.Authorization;
using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Auth;

public class RequireRoleAttribute : AuthorizeAttribute
{
    public RequireRoleAttribute(params Role[] roles)
    {
        Policy = $"RolePolicy:{string.Join(",", roles)}";
    }
}