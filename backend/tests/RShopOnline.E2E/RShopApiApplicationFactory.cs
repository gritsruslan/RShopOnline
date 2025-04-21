using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RShopAPI_Test.Storage;
using Testcontainers.PostgreSql;

namespace RShopOnline.E2E;

public class RShopApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder().Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            ["ConnectionStrings:Database"] = postgreSqlContainer.GetConnectionString()
        }!).Build();
        builder.UseConfiguration(configuration);
        base.ConfigureWebHost(builder);
    }
    
    public async Task InitializeAsync()
    {
        await postgreSqlContainer.StartAsync();

        var rshopDbContext = new RShopDbContext(
            new DbContextOptionsBuilder<RShopDbContext>().UseNpgsql(postgreSqlContainer.GetConnectionString()).Options);
        await rshopDbContext.Database.MigrateAsync();
    }

    public new async Task DisposeAsync() => await postgreSqlContainer.DisposeAsync();
}