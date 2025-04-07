using Microsoft.AspNetCore.Authorization;
using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Auth;

public class RoleRequirements(Role[] roles) : IAuthorizationRequirement
{
    public Role[] Roles { get; set; } = roles;
}