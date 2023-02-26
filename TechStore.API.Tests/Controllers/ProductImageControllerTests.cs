using Moq;
using System.Text;
using TechStore.DAL.Repos;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace TechStore.API.Tests.Controllers;

public class ProductImageControllerTests : IClassFixture<ProductImageControllerFixture>
{
    private readonly ProductImageControllerFixture _fixture;

    public ProductImageControllerTests(ProductImageControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void GetReturnsNotFoundNonExistingId()
    {
        var getResult = _fixture.Controller.Get(1);

        Assert.IsType<NotFoundResult>(getResult);
    }

    [Fact]
    public void GetReturnsFileContentResult()
    {
        var getResult = _fixture.Controller.Get(0);

        Assert.IsType<FileContentResult>(getResult);

        var service = _fixture.Service.Object;
        var repo = _fixture.Repository.Object;

        var fileContentResult = getResult as FileContentResult;

        using var expectedFileStream = service.GetFile("helloworld.txt");
        var expectedBytes = new byte[expectedFileStream.Length];
        expectedFileStream.Read(expectedBytes);

        var productImage = repo.Get(0);

        var expectedContentType = productImage!.ImageContentType;

        fileContentResult!.FileContents.Should().BeEquivalentTo(expectedBytes);
        fileContentResult!.ContentType.Should().BeEquivalentTo(expectedContentType);
    }
}
