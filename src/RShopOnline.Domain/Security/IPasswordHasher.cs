namespace RShopAPI_Test.Services.Security;

public interface IPasswordHasher
{
    byte[] HashPassword(string password, byte[] salt);
    
    bool VerifyPassword(string password, byte[] passwordHash, byte[] salt);
}