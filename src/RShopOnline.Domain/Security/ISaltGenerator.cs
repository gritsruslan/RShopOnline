namespace RShopAPI_Test.Services.Security;

public interface ISaltGenerator
{
    byte[] Generate();
}