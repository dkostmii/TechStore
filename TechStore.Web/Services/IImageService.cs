namespace TechStore.Web.Services;

public interface IImageService
{
    Stream GetFile(string filePath);
    void CheckImageFiles(IFormFileCollection files);
    (string filePath, string contentType) UploadFile(IFormFile file);
    (string filePath, string contentType) ReplaceFile(IFormFile file, string filePath);
    void RemoveFile(string filePath);
}
