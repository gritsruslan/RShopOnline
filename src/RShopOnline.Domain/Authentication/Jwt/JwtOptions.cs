namespace RShopAPI_Test.Services.Authentication.Jwt;

public class JwtOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpiresHours { get; set; }
    public required string SecretKey { get; set; }
}