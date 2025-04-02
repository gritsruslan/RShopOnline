using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Services.Jwt;

public interface IJwtProvider
{
    string GenerateToken(User user);
}