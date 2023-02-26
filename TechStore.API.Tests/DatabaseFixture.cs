using Microsoft.EntityFrameworkCore;
using TechStore.DAL;

namespace TechStore.API.Tests;

public abstract class DatabaseFixture
{
    private const string ConnectionString = @"DataSource=file::memory:";

    public DbShopContext CreateContext()
        => new DbShopContext(
            new DbContextOptionsBuilder<DbShopContext>()
            .UseSqlite(ConnectionString)
            .Options);

    public DatabaseFixture()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Cleanup();
    }

    public void Cleanup()
    {
        using var context = CreateContext();

        SeedData(context);

        context.SaveChanges();
    }

    public abstract void SeedData(DbShopContext context);
}
