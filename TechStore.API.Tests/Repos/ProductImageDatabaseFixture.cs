using TechStore.DAL;
using TechStore.DAL.Models;

namespace TechStore.API.Tests.Repos;

public class ProductImageDatabaseFixture : DatabaseFixture
{
    public override void SeedData(DbShopContext context)
    {
        context.ProductImages.RemoveRange(context.ProductImages);

        context.ProductImages.AddRange(
            new ImageEntity { FilePath = "ProductImages/hello.jpg", ContentType = "image/jpeg" },
            new ImageEntity { FilePath = "ProductImages/hello.png", ContentType = "image/png" }
        );
    }
}
