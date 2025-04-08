namespace RShopAPI_Test.Services.Interfaces;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
}