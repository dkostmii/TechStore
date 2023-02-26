namespace TechStore.Domain.Models;

public class ImageEntityDTO : BaseEntityDTO
{
    public Uri? ImageUrl { get; set; }
    public required string ImageContentType { get; set; }
}
