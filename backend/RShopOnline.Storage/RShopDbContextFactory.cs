using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RShopAPI_Test.Storage;

public class RShopDbContextFactory : IDesignTimeDbContextFactory<RShopDbContext>
{
    public RShopDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RShopDbContext>();
        optionsBuilder.UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=postgres");
        return new RShopDbContext(optionsBuilder.Options);
    }
}