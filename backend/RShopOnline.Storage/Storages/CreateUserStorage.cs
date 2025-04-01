using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class CreateUserStorage(RShopDbContext dbContext) : ICreateUserStorage
{
    public async Task CreateUser(
        string name, 
        string email, 
        byte[] passwordHash,
        byte[] salt, 
        UserRole role, 
        CancellationToken ct)
    {
        await dbContext.Users.AddAsync(
            new UserEntity() {Name = name, Email = email, PasswordHash = passwordHash, Salt = salt, Role = role}, ct);
        
        await dbContext.SaveChangesAsync(ct);
    }
}