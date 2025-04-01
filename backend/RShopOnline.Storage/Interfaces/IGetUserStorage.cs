using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IGetUserStorage
{
    Task<User?> GetUser(string email, CancellationToken ct);
    
    Task<bool> DoesUserExist(string email, CancellationToken ct);
}