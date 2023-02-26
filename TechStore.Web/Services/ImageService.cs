using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace TechStore.Web.Services;

public class ImageService : IImageService
{
    private readonly ImageServiceOptions _opts;
    private readonly IFileSystem _fs;
    private readonly string _path;

    public ImageService(IOptions<ImageServiceOptions> options, IFileSystem fs, IWebHostEnvironment env)
    {
        _opts = options.Value;
        _fs = fs;

        _path = _fs.Path.Join(env.ContentRootPath, _opts.ProductImagesPath);

        if (!_fs.Directory.Exists(_path))
            _fs.Directory.CreateDirectory(_path);
    }

    public Stream GetFile(string filePath)
    {
        if (!_fs.File.Exists(filePath))
            throw new FileNotFoundException($"File {filePath} does not exist.");

        return _fs.File.OpenRead(filePath);
    }

    private (Stream s, IImageFormat fmt) CompressImage(IFormFile imageFile)
    {
        using var stream = imageFile.OpenReadStream();

        using var image = Image.Load(stream);

        var aspectRatio = (decimal)image.Width / image.Height;

        if (_opts.DefaultLargestImageSide > 0)
        {
            image.Mutate(i => {
                if (aspectRatio >= 1)
                {
                    i.Resize(_opts.DefaultLargestImageSide, 0);
                }
                else
                {
                    i.Resize(0, _opts.DefaultLargestImageSide);
                }
            });
        }

        var resultStream = new MemoryStream();

        var jpegEncoder = new JpegEncoder
        {
            Quality = _opts.CompressionQuality,
        };

        image.SaveAsJpeg(resultStream, jpegEncoder);
        resultStream.Seek(0, SeekOrigin.Begin);

        var fmt = Image.DetectFormat(resultStream);
        resultStream.Seek(0, SeekOrigin.Begin);

        return (resultStream, fmt);
    }

    private void CheckImageFile(IFormFile imageFile)
    {
        if (!_opts.AllowedContentTypes.Any(contentType => contentType == imageFile.ContentType))
            throw new ArgumentException($"Invalid content type: {imageFile}");

        if (imageFile.Length > _opts.MaxFileSize)
            throw new ArgumentException($"Image file must have size less than {_opts.MaxFileSize} bytes.");

        using var stream = imageFile.OpenReadStream();

        var imageData = Image.Identify(stream);

        var largestSide = imageData.Width >= imageData.Height ? imageData.Width : imageData.Height;

        if (largestSide > _opts.MaxLargestImageSide)
            throw new ArgumentException($"The largest side of image should be less or equal to {_opts.MaxLargestImageSide} pixels. Got {largestSide}");
    }

    private string SanitizeFileName(string fileName)
    {
        var invalidFileNameChars = new string(_fs.Path.GetInvalidFileNameChars());
        string invalidChars = Regex.Escape(invalidFileNameChars);
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return Regex.Replace(fileName, invalidRegStr, "_");
    }

    public void CheckImageFiles(IFormFileCollection files)
    {
        if (files.Count == 0)
            throw new ArgumentException("No files provided.");

        var exceptions = new List<Exception>();

        foreach (var file in files)
        {
            try
            {
                CheckImageFile(file);
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }

        if (exceptions.Count > 0)
            throw new AggregateException("Error uploading files: ", exceptions);
    }


    public (string filePath, string contentType) UploadFile(IFormFile file)
    {
        CheckImageFile(file);
        /*
        var sanitizedFileName = SanitizeFileName(file.FileName);
        var fileExtension = _fs.Path.GetExtension(sanitizedFileName);
        */

        var (compressed, fmt) = CompressImage(file);

        var extension = fmt.FileExtensions.First();

        if (!extension.StartsWith("."))
            extension = "." + extension;

        var randomFileName = _fs.Path.GetRandomFileName() + extension;

        var writeFilePath = _fs.Path.GetFullPath(_fs.Path.Join(_path, randomFileName));
        using var writeStream = _fs.File.Create(writeFilePath);

        using (compressed)
        {
            compressed.CopyTo(writeStream);
        }

        return (writeFilePath, fmt.DefaultMimeType);
    }

    public (string filePath, string contentType) ReplaceFile(IFormFile file, string filePath)
    {
        var result = UploadFile(file);
        RemoveFile(filePath);

        return result;
    }

    public void RemoveFile(string filePath)
    {
        if (!_fs.File.Exists(filePath))
            throw new FileNotFoundException($"File {filePath} does not exist.");

        _fs.File.Delete(filePath);
    }
}
