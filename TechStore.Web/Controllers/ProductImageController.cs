using Microsoft.AspNetCore.Mvc;
using TechStore.DAL.Repos;
using TechStore.Domain.Interfaces;
using TechStore.Domain.Models;
using TechStore.Web.Services;

namespace TechStore.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductImageController : ControllerBase
{
    private readonly IDataRepository<ImageEntityDTO> _repo;
    private readonly IImageService _service;

    public ProductImageController(IDataRepository<ImageEntityDTO> repo, IImageService service)
    {
        _repo = repo;
        _service = service;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        // FIXME: This endpoint returns a file

        var result = _repo.Get(id) as ImagePrivateEntityDTO;

        if (result is null)
            return NotFound();

        using var imageStream = _service.GetFile(result.ImageFilePath);

        var bytes = new byte[imageStream.Length];

        imageStream.Read(bytes);

        return File(bytes, result.ImageContentType!);
    }

    [HttpPost]
    public IActionResult Post([FromForm] IFormFileCollection files)
    {
        var contentType = Request.ContentType;

        if (!contentType!.StartsWith("multipart/form-data"))
        {
            return new UnsupportedMediaTypeResult();
        }

        try
        {
            _service.CheckImageFiles(files);
        } catch (AggregateException e)
        {
            return BadRequest(e);
        }

        var images = files.Select(file =>
        {
            var (filePath, fileContentType) = _service.UploadFile(file);
            return _repo.Insert(new ImagePrivateEntityDTO
            {
                ImageFilePath = filePath,
                ImageContentType = fileContentType,
            });
        });

        return Ok(images);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromForm] IFormFileCollection files)
    {
        var contentType = Request.ContentType;

        if (!contentType!.StartsWith("multipart/form-data"))
            return new UnsupportedMediaTypeResult();

        var found = _repo.Get(id) as ImagePrivateEntityDTO;

        if (found is null)
            return NotFound();

        if (files.Count < 1)
            return BadRequest("No file provided");

        if (files.Count > 1)
            return BadRequest("Single file expected");

        try
        {
            var (filePath, fileContentType) = _service.ReplaceFile(files[0], found.ImageFilePath!);
            found.ImageFilePath = filePath;
            found.ImageContentType = fileContentType;

            var result = _repo.Update(found)!;
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var found = _repo.Get(id) as ImagePrivateEntityDTO;

        if (found is null)
            return NotFound();

        try
        {
            _service.RemoveFile(found.ImageFilePath);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

        _repo.Delete(id);

        return NoContent();
    }
}
