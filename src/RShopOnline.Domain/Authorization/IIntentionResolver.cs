using RShopAPI_Test.Services.Authentication;

namespace RShopAPI_Test.Services.Authorization;

public interface IIntentionResolver;

public interface IIntentionResolver<TIntentionCommand> : IIntentionResolver
{
    public bool IsAllowed(IIdentity identity);
}