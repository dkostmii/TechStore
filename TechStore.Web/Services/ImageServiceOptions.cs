namespace TechStore.Web.Services;

public class ImageServiceOptions
{
    public const string ImageService = "ImageService";

    public string ProductImagesPath { get; set; } = "";
    public int MaxFileSize { get; set; }
    public int MaxLargestImageSide { get; set; }
    public int DefaultLargestImageSide { get; set; }
    public string[] AllowedContentTypes { get; set; } = new string[] { };
    public int CompressionQuality { get; set; } = 85;
}
