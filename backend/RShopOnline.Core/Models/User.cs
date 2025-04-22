using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Core.Models;

public class User
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Email { get; set; }

    public required byte[] PasswordHash { get; set; }

    public required byte[] Salt { get; set; }
    
    public Role Role { get; set; }
}