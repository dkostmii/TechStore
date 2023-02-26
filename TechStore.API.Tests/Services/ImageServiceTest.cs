using Microsoft.Extensions.Options;
using Moq;
using TechStore.Web.Services;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.AspNetCore.Hosting;

namespace TechStore.API.Tests.Services;

internal class ImageServiceFixture : IDisposable
{
    public ImageService Service { get; private set; }

    public ImageServiceFixture()
    {
        var optionsMock = new Mock<IOptions<ImageServiceOptions>>();
        optionsMock
            .SetupGet(o => o.Value)
            .Returns(new ImageServiceOptions
            {
                MaxFileSize = 262144,
                MaxLargestImageSide = 128,
                DefaultLargestImageSide = 64,
                AllowedContentTypes = new[] { "image/png" },
                ProductImagesPath = "ProductImages"
            });

        var mockFs = new MockFileSystem();

        var envMock = new Mock<IWebHostEnvironment>();
        envMock
            .SetupGet(env => env.ContentRootPath)
            .Returns("./");

        Service = new ImageService(optionsMock.Object, mockFs, envMock.Object);
    }

    public void Dispose()
    {
    }
}

internal class ImageServiceTest : IClassFixture<ImageServiceFixture>
{
    private readonly ImageServiceFixture _fixture;

    public ImageServiceTest(ImageServiceFixture fixture)
    {
        _fixture = fixture;
    }

    // TODO: Write tests
}
