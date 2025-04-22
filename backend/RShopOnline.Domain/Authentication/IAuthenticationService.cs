namespace RShopAPI_Test.Services.Authentication;

public interface IAuthenticationService
{
    Task<IIdentity> Authenticate(Guid userId);
}