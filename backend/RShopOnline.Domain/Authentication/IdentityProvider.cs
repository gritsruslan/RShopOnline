using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Interfaces;

namespace RShopAPI_Test.Services.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; } = null!;
}