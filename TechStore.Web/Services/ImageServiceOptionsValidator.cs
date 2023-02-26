using FluentValidation;
using System.IO.Abstractions;

namespace TechStore.Web.Services;

public class ImageServiceOptionsValidator : AbstractValidator<ImageServiceOptions>
{
    public ImageServiceOptionsValidator(IFileSystem fs, IWebHostEnvironment env)
    {
        var imageContentTypes = new[]
        {
             "image/gif",
             "image/jpeg",
             "image/png",
             "image/tiff",
             "image/vnd.microsoft.icon",
             "image/x-icon",
             "image/vnd.djvu",
             "image/svg+xml",
        };

        RuleFor(x => x.AllowedContentTypes)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(allowedContentTypes =>
                allowedContentTypes.All(contentType => imageContentTypes.Any(c => c == contentType)))
            .WithMessage("Allowed content types must be non-empty array of valid content types, e.g. image/jpeg, image/png etc.");

        RuleFor(x => x.MaxLargestImageSide)
            .NotEmpty()
            .InclusiveBetween(1, 10240)
            .WithMessage("Max largest image side must be in range [1, 10240]");

        RuleFor(x => x.MaxFileSize)
            .NotEmpty()
            .InclusiveBetween(1, 20_971_520)
            .WithMessage("Max file size should be in range [1, 20971520] bytes");

        RuleFor(x => x.DefaultLargestImageSide)
            .InclusiveBetween(0, 10240)
            .WithMessage("Default largest image side should be in range [0, 10240]");

        RuleFor(x => x.ProductImagesPath)
            .Must(productImagesPath => !(productImagesPath.Contains('.') || productImagesPath.StartsWith("/") || productImagesPath.StartsWith("\\")))
            .WithMessage("Product images path must be descendant. It can not refer to parent folders through .. and root folder through /");

        RuleFor(x => x.ProductImagesPath)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Must(productImagesPath => IsValidPath(productImagesPath, fs))
            .Must(productImagesPath => IsAccessible(productImagesPath, fs, env))
            .WithMessage("Product images path must contain valid characters and be accessible");

        RuleFor(x => x.CompressionQuality)
            .NotEmpty()
            .InclusiveBetween(1, 100)
            .WithMessage("Compression quality must be in range [1, 100] percent");
    }

    private static bool IsValidPath(string path, IFileSystem fs)
    {
        var invalidChars = fs.Path.GetInvalidPathChars();

        var result = !path.Any(c => invalidChars.Contains(c));

        return result;
    }

    private static bool IsAccessible(string path, IFileSystem fs, IWebHostEnvironment env)
    {
        path = fs.Path.Join(env.ContentRootPath, path);

        var dummyFilePath = fs.Path.Join(path, "dummy.txt");
        var dummyFileContent = "Hello, World!";

        try
        {
            var dirExisted = false;

            if (fs.Directory.Exists(path))
                dirExisted = true;
            else
                fs.Directory.CreateDirectory(path);

            fs.File.WriteAllText(dummyFilePath, dummyFileContent);

            if (fs.File.Exists(dummyFilePath))
            {
                // Cleanup

                fs.File.Delete(dummyFilePath);

                if (dirExisted)
                    fs.Directory.Delete(path, true);

                return true;
            }

            return false;
        }
        catch (IOException)
        {
            // Catch only IOExceptions
            return false;
        }
        catch (Exception)
        {
            // Something bad happened
            throw;
        }
    }
}
