using RShopAPI_Test.Core.Enums;

namespace RShopAPI_Test.Services.Authentication;

public interface IIdentity
{
    Guid Id { get; }
    
    Role Role { get; }
}