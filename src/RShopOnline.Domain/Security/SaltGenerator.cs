using System.Security.Cryptography;

namespace RShopAPI_Test.Services.Security;

public class SaltGenerator : ISaltGenerator
{
    private const int CountOfBytes = 32;
    
    public byte[] Generate() => RandomNumberGenerator.GetBytes(CountOfBytes);
}