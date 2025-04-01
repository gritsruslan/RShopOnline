namespace RShopAPI_Test.Services.Jwt;

public class AuthSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required TimeSpan Expires { get; set; }
    public required string SecretKey { get; set; }
}