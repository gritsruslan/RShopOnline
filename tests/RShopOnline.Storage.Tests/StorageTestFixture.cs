using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RShopAPI_Test.Storage;
using Testcontainers.PostgreSql;

namespace RShopOnline.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresSqlContainer = new PostgreSqlBuilder().Build();

    internal RShopDbContext GetDbContext() => new(new DbContextOptionsBuilder<RShopDbContext>()
            .UseNpgsql(_postgresSqlContainer.GetConnectionString()).Options);

    internal IMapper GetMapper() => new Mapper(new MapperConfiguration(cfg =>
        cfg.AddMaps(typeof(RShopDbContext).Assembly)));

    public virtual async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();
        var rshopDbContext = GetDbContext();
        await rshopDbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _postgresSqlContainer.DisposeAsync();
}