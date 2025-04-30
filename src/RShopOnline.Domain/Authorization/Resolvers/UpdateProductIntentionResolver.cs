using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;

namespace RShopAPI_Test.Services.Authorization.Resolvers;

public class UpdateProductIntentionResolver : IIntentionResolver<UpdateProductIntentionResolver>
{
    public bool IsAllowed(IIdentity identity) => identity.Role is Role.Admin or Role.Manager;
}