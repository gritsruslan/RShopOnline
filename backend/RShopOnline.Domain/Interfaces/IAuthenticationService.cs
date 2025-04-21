using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Services.Authentication;

namespace RShopAPI_Test.Services.Interfaces;

public interface IAuthenticationService
{
    Task<IIdentity> Authenticate(Guid userId);
}