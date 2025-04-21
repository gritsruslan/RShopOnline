using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Services.Authentication;
using RShopAPI_Test.Services.Commands;

namespace RShopAPI_Test.Services.Authorization;

public interface IIntentionManager
{
    public bool IsAllowed<TIntentionCommand>();
}
