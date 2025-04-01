using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RShopAPI_Test.Services.Security;

public class PasswordHasher : IPasswordHasher
{
    public byte[] HashPassword(string password, byte[] salt) =>
         KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 5000, 32);

    public bool VerifyPassword(string password, byte[] passwordHash, byte[] salt) =>
         HashPassword(password, salt).Equals(passwordHash);

}