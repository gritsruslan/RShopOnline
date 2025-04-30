using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Authorization.Resolvers;

public class CreateProductIntentionResolver : IIntentionResolver<CreateProductCommand>
{
    public bool IsAllowed(IIdentity identity) => identity.Role is Role.Admin or Role.Manager;
}