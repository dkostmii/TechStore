using Microsoft.AspNetCore.Mvc;
using TechStore.Domain.Interfaces;
using TechStore.Domain.Models;

namespace TechStore.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IDataRepository<ProductDTO> _repo;

    public ProductController(IDataRepository<ProductDTO> repo)
    {
        _repo = repo;
    }

    public IActionResult Get(int id)
    {
        var result = _repo.Get(id);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
    
    public IActionResult Post(ProductDTO product)
    {
        try
        {
            // FIXME: Validate ProductDTO before inserting in repo
            var result = _repo.Insert(product);

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IActionResult Put(int id, ProductDTO product)
    {
        try
        {
            // FIXME: Repo should accept id parameter here
            // FIXME: Validate product before updating in repo
            var result = _repo.Update(product);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IActionResult Delete(int id)
    {
        var found = _repo.Get(id);

        if (found is null)
            return NotFound();
        
        try
        {
            _repo.Delete(id);
        }
        catch (ArgumentNullException e)
        {
            return StatusCode(500, e);
        }
        catch (Exception)
        {
            throw;
        }

        return NoContent();
    }
}

