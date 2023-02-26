using AutoMapper;
using Microsoft.Extensions.Options;
using TechStore.DAL.Models;
using TechStore.Domain.Models;

namespace TechStore.Web.Mappers;

public class ShopProfile : Profile
{
    private readonly ShopProfileOptions _opts;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="config">
    /// <b>Injected</b> WebApplication configuration.
    /// <br></br>
    /// <br></br>
    /// Must have defined <c>"Host"</c> and <c>"ProductImagesEndpoint"</c> properties defined.
    /// Throws <see cref="ArgumentNullException"/> otherwise.
    /// </param>
    /// <exception cref="ArgumentNullException"></exception>
    public ShopProfile(IOptions<ShopProfileOptions> options)
    {
        _opts = options.Value;

        MapBaseModels();
        MapLeafModels();
        MapComplexModels();
    }

    private void MapBaseModels()
    {
        CreateMap<BaseEntity, BaseEntityDTO>().IncludeAllDerived().ReverseMap();
        CreateMap<LoginEntity, LoginEntityDTO>().IncludeAllDerived().ReverseMap();
        CreateMap<UserEntity, UserEntityDTO>().IncludeAllDerived().ReverseMap();

        CreateMap<ImageEntity, ImageEntityDTO>()
            .ForMember(dest => dest.ImageUrl,
                member => member.MapFrom(image => GetImageUrl(image.Id)))
            .ForMember(dest => dest.ImageContentType, member => member.MapFrom(image => image.ContentType));

        CreateMap<ImageEntity, ImagePrivateEntityDTO>()
            .IncludeBase<ImageEntity, ImageEntityDTO>()
            .ForMember(dest => dest.ImageFilePath, member => member.MapFrom(image => image.FilePath));

        CreateMap<ImagePrivateEntityDTO, ImageEntity>()
            .ForMember(dest => dest.FilePath, member => member.MapFrom(image => image.ImageFilePath))
            .ForMember(dest => dest.ContentType, member => member.MapFrom(image => image.ImageContentType))
            .IncludeAllDerived();
    }

    private void MapLeafModels()
    {
        CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
        CreateMap<Company, CompanyDTO>().ReverseMap();
        CreateMap<Review, ReviewDTO>().ReverseMap();
    }

    private void MapComplexModels()
    {
        CreateMap<Admin, AdminDTO>()
            .ForMember(dest => dest.Username, member => member.MapFrom(admin => admin.UserName))
            .ReverseMap();

        CreateMap<Client, ClientDTO>().ReverseMap();

        CreateMap<Order, OrderDTO>().ReverseMap();

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.UnitPrice, member => member.MapFrom(product => product.Price))
            .ForMember(dest => dest.UnitsAvailable, member => member.MapFrom(product => product.Available))
            .ForMember(dest => dest.ProducingCountry, member => member.MapFrom(product => product.Country))
            .ReverseMap();
    }

    private Uri GetImageUrl(int id)
    {
        var builder = new UriBuilder(_opts.ProductImagesEndpoint!);

        if (!builder.Path.EndsWith("/"))
            builder.Path += "/";

        builder.Path += id.ToString();

        return builder.Uri;
    }
}
