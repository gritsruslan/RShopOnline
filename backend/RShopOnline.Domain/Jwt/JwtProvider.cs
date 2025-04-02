using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.Jwt;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    public string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new("userId", user.Id.ToString()),
            new("userRole", user.Role.ToString()),
        ];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.SecretKey)),
            SecurityAlgorithms.HmacSha256Signature);

        var jwtToken = new JwtSecurityToken(
            claims: claims,
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            expires: DateTime.UtcNow.AddHours(options.Value.ExpiresHours),
            signingCredentials: signingCredentials
            );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}