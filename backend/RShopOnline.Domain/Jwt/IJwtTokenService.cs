using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.Jwt;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}