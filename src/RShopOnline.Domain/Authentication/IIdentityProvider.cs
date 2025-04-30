namespace RShopAPI_Test.Services.Authentication;

public interface IIdentityProvider
{
    public IIdentity Current { get; set; }
}