namespace RShopAPI_Test.Services.Authorization;

public interface IIntentionManager
{
    public bool IsAllowed<TIntentionCommand>();
}
