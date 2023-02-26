using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechStore.DAL;
using TechStore.DAL.Repos;
using TechStore.Web.Mappers;
using TechStore.Web.Services;

using System.IO.Abstractions;
using TechStore.Web.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DbShopContext>(db =>
    db.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("TechStore.API")));

// Inject Repos
builder.Services.AddTransient<ProductImageRepository>();

builder.Services.AddSingleton<IFileSystem, FileSystem>();

// Inject option validators
builder.Services.AddSingleton<IValidator<ShopProfileOptions>, ShopProfileOptionsValidator>();
builder.Services.AddSingleton<IValidator<ImageServiceOptions>, ImageServiceOptionsValidator>();

builder.Services
    .AddOptions<ImageServiceOptions>()
    .Bind(builder.Configuration.GetRequiredSection(ImageServiceOptions.ImageService))
    .ValidateFluently()
    .ValidateOnStart();

builder.Services
    .AddOptions<ShopProfileOptions>()
    .Bind(builder.Configuration.GetRequiredSection(ShopProfileOptions.ShopProfile))
    .ValidateFluently()
    .ValidateOnStart();

builder.Services.AddScoped<ImageService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject ShopProfile for AutoMapper
builder.Services.AddSingleton(provider =>
{
    return new MapperConfiguration(config =>
    {
        config.AddProfile(new ShopProfile(provider.GetService<IOptions<ShopProfileOptions>>()!));
    }).CreateMapper();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var env = app.Services.GetRequiredService<IWebHostEnvironment>();
    var imageServiceOptions = app.Services.GetRequiredService<IOptions<ImageServiceOptions>>().Value;
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    var scope = app.Services.CreateScope();

    var fs = scope.ServiceProvider.GetRequiredService<IFileSystem>();

    var productImagesPath = fs.Path.Join(env.ContentRootPath, imageServiceOptions.ProductImagesPath);

    if (fs.Directory.Exists(productImagesPath))
    {
        fs.Directory.Delete(productImagesPath, true);
    }

    var dbContext = scope.ServiceProvider.GetRequiredService<DbShopContext>();

    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    logger.LogInformation("Product images folder {productImagesPath}", productImagesPath);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
