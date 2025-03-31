using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Core.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = [];

    public byte[] Salt { get; set; } = [];
    
    public UserRole Role { get; set; }
}