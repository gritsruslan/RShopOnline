using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Auth;

public class RolePolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider = new(options);

    private const string PolicyPrefix = "RolePolicy:";
    
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PolicyPrefix))
        {
            var rolePart = policyName.Substring(PolicyPrefix.Length);
            var roles = rolePart.Split(',')
                .Select(Enum.Parse<Role>)
                .ToArray();

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new RoleRequirements(roles))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallbackPolicyProvider.GetFallbackPolicyAsync();
    }
}