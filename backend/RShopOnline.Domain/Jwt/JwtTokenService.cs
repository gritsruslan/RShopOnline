using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.Jwt;

public class JwtTokenService(IOptions<AuthSettings> options) : IJwtTokenService
{
    public string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new("userId", user.Id.ToString()),
            new("email", user.Email),
            new("userRole", user.Role.ToString()),
        ];

        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            claims: claims,
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256Signature)
            );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}