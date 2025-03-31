using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Storage.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;
    
    public UserRole Role { get; set; }
    
    public ICollection<OrderEntity> Orders { get; set; } = null!;
}