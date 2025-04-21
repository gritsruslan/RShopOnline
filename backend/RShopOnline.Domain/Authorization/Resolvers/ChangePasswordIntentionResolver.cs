using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Authorization.Resolvers;

public class ChangePasswordIntentionResolver : IIntentionResolver<ChangePasswordCommand>
{
    public bool IsAllowed(IIdentity identity) =>
        identity.Role != Role.Guest;
}