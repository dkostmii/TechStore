using Microsoft.Extensions.Options;
using Moq;
using TechStore.Web.Services;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TechStore.API.Tests.Services;

public class ImageServiceFixture : IDisposable
{
    public ImageService Service { get; private set; }
    private IList<IFormFile> TestImages { get; set; }

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

        TestImages = new List<IFormFile>();

        var realFs = new FileSystem();

        var testImagesPaths = realFs.Directory.EnumerateFiles("./TestImages");

        var mimeMapping = new Dictionary<string, string>
        {
            { ".txt", "text/plain" },
            { ".png", "image/png" }
        };

        foreach (var testImagePath in testImagesPaths)
        {
            using (var stream = realFs.File.OpenRead(testImagePath))
            {
                var testImageFileName = realFs.Path.GetFileName(testImagePath);
                var testImageFileExtension = realFs.Path.GetExtension(testImagePath);
                var testImageName = realFs.Path.GetFileNameWithoutExtension(testImagePath);

                var imageFile = new FormFile(stream, 0, stream.Length, testImageName, testImageFileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = mimeMapping[testImageFileExtension]
                };

                TestImages.Add(imageFile);
            }
        }
    }

    public IFormFile GetTestImage(string imageName)
    {
        return TestImages.Where(imageFile => imageFile.Name == imageName).First();
    }

    public void Dispose()
    {
    }
}

public class ImageServiceTest : IClassFixture<ImageServiceFixture>
{
    private readonly ImageServiceFixture _fixture;

    public ImageServiceTest(ImageServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void CheckImageFilesNoImagesThrows()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _fixture.Service.CheckImageFiles(new FormFileCollection());
        });
    }

    [Fact]
    public void CheckImageFileInvalidContentTypeThrows()
    {
        var testImage = _fixture.GetTestImage("NotImage");
        var fileCollection = new FormFileCollection
        {
            testImage
        };

        Assert.Throws<AggregateException>(() =>
        {
            _fixture.Service.CheckImageFiles(fileCollection);
        });
    }
}
