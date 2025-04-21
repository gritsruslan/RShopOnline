using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authentication;

namespace RShopAPI_Test.Services.Interfaces;

public interface IIdentityProvider
{
    public IIdentity Current { get; set; }
}