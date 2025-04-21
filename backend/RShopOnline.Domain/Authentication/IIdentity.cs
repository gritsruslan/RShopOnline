using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Authentication;

public interface IIdentity
{
    Guid Id { get; set; }
    
    Role Role { get; set; }
}

public class UserIdentity(Guid id, Role role) : IIdentity
{
    public Guid Id { get; set; } = id;

    public Role Role { get; set; } = role;
    
    public static UserIdentity Guest => new UserIdentity(Guid.Empty, Role.Guest);
}