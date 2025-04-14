using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;

namespace RShopAPI_Test.Storage.Interfaces;

public interface IUsersRepository
{
    public Task CreateUser(
        string name, 
        string email, 
        byte[] passwordHash,
        byte[] salt, 
        Role role, 
        CancellationToken ct);
    
    public Task<User?> GetUserByEmail(string email, CancellationToken ct);
    
    public Task<User?> GetUserById(Guid id, CancellationToken ct);
    
    public Task<bool> UserExists(string email, CancellationToken ct);
    
    public Task UpdatePassword(byte[] newPasswordHash, CancellationToken ct);
}