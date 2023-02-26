using Microsoft.EntityFrameworkCore;
using TechStore.DAL.Models;

namespace TechStore.DAL;

public class DbShopContext : DbContext
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Company> Suppliers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ImageEntity> ProductImages { get; set; }

    public DbShopContext(DbContextOptions<DbShopContext> dbContextOptions) : base(dbContextOptions)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Admin>()
            .HasIndex(admin => admin.UserName)
            .IsUnique();

        builder.Entity<Admin>()
            .HasIndex(admin => admin.Email)
            .IsUnique();

        builder.Entity<Client>()
            .HasIndex(client => client.Email)
            .IsUnique();

        builder.Entity<Company>()
            .HasIndex(supplier => supplier.Name)
            .IsUnique();

        builder.Entity<ImageEntity>()
            .HasIndex(image => image.FilePath)
            .IsUnique();
    }
}
