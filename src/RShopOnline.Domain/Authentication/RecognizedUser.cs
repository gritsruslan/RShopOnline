using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Authentication;

public class RecognizedUser(Guid id, Role role) : IIdentity
{
    public Guid Id { get; } = id;

    public Role Role { get; } = role;
    
    public static RecognizedUser Guest => new(Guid.Empty, Role.Guest);
}