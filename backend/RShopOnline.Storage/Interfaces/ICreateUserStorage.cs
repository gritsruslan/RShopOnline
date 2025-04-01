using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface ICreateUserStorage
{
    Task CreateUser(string name, string email, byte[] passwordHash, byte[] salt, UserRole role, CancellationToken ct);
}