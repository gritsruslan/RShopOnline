using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Core.Models;
using RShopAPI_Test.Storage.Interfaces;

namespace RShopAPI_Test.Storage.Storages;

public class GetUserStorage(RShopDbContext dbContext, IMapper mapper) : IGetUserStorage
{
    public async Task<User?> GetUser(string email, CancellationToken ct)
    {
        return await dbContext.Users
            .Where(u => u.Email == email)
            .Select(u => mapper.Map<User>(u))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> DoesUserExist(string email, CancellationToken ct)
    {
        return await GetUser(email, ct) != null;
    }
}