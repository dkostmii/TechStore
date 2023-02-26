using Moq;
using System.Text;
using TechStore.Domain.Interfaces;
using TechStore.Domain.Models;
using TechStore.Web.Controllers;
using TechStore.Web.Services;

namespace TechStore.API.Tests.Controllers;

public class ProductImageControllerFixture : IDisposable
{
    public ProductImageController Controller { get; private set; }
    public Mock<IImageService> Service { get; private set; }
    public Mock<IDataRepository<ImageEntityDTO>> Repository { get; private set; }

    public ProductImageControllerFixture()
    {
        Service = new Mock<IImageService>();

        Service
            .Setup(service => service.GetFile(It.IsAny<string>()))
            .Returns(() =>
            {
                var content = "Hello, World!";
                return new MemoryStream(Encoding.UTF8.GetBytes(content));
            });

        Repository = new Mock<IDataRepository<ImageEntityDTO>>();

        Repository
            .Setup(repo => repo.Get(It.Is<int>(id => id == 0)))
            .Returns(
                new ImagePrivateEntityDTO
                {
                    Id = 0,
                    ImageContentType = "text/plain",
                    ImageFilePath = "helloworld.txt",
                    ImageUrl = new Uri("http://helloworld/ProductImages/0"),
                }
            );

        Repository
            .Setup(repo => repo.Get(It.Is<int>(id => id != 0)))
            .Returns(() => null);

        Controller = new ProductImageController(Repository.Object, Service.Object);
    }

    public void Dispose()
    {
    }
}
