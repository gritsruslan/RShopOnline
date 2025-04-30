using RShopAPI_Test.Services.Authentication;

namespace RShopAPI_Test.Services.Authorization;

public class IntentionManager(
    IEnumerable<IIntentionResolver> resolvers,
    IIdentityProvider identityProvider) 
    : IIntentionManager
{
    public bool IsAllowed<TIntentionCommand>()
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntentionCommand>>().FirstOrDefault();
        return matchingResolver?.IsAllowed(identityProvider.Current) ?? false;
    }
}