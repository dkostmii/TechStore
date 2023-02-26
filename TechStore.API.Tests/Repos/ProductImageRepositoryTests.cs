using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using TechStore.DAL.Repos;
using TechStore.Domain.Models;
using TechStore.Web.Mappers;
using FluentAssertions;

namespace TechStore.API.Tests.Repos;

public class ProductImageRepositoryTests : IClassFixture<ProductImageDatabaseFixture>, IDisposable
{
    private readonly IMapper _mapper;
    private readonly ProductImageDatabaseFixture _fixture;

    public ProductImageRepositoryTests(ProductImageDatabaseFixture fixture)
    {
        var optionsMock = new Mock<IOptions<ShopProfileOptions>>();
        optionsMock.SetupGet(o => o.Value)
            .Returns(new ShopProfileOptions
            {
                ProductImagesEndpoint = new Uri("http://helloworld/productImages")
            });

        _mapper = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ShopProfile(optionsMock.Object));
        }).CreateMapper();

        _fixture = fixture;
    }

    [Fact]
    public void GetReturnsValidResult()
    {
        using var context = _fixture.CreateContext();

        var first = context.ProductImages.First();
        var expected = _mapper.Map<ImagePrivateEntityDTO>(first);

        var testedRepo = new ProductImageRepository(context, _mapper);

        var actual = testedRepo.Get(first.Id);

        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);
        actual!.GetType().Should().BeSameAs(expected.GetType());
    }

    public void Dispose()
    {
        _fixture.Cleanup();
    }
}
