using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Enums;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Entities;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Repositories;

public class UsersRepository(RShopDbContext dbContext, IMapper mapper) : IUsersRepository
{
    public async Task CreateUser(
        string name, 
        string email, 
        byte[] passwordHash,
        byte[] salt, 
        Role role, 
        CancellationToken ct)
    {
        await dbContext.Users.AddAsync(
            new UserEntity() {Name = name, Email = email, PasswordHash = passwordHash, Salt = salt, Role = role}, ct);
        
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task<User?> GetUserByEmail(string email, CancellationToken ct)
    {
        return await dbContext.Users
            .Where(u => u.Email == email)
            .Select(u => mapper.Map<User>(u))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<User?> GetUserById(Guid id, CancellationToken ct)
    {
        return await dbContext.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => mapper.Map<User>(u))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> UserExists(string email, CancellationToken ct)
    {
        return await GetUserByEmail(email, ct) != null;
    }
}