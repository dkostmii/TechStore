using TechStore.Domain.Interfaces;
using TechStore.Domain.Models;
using TechStore.DAL.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace TechStore.DAL.Repos;

public class ProductImageRepository : IDataRepository<ImageEntityDTO>
{
    private readonly DbShopContext _context;
    private readonly IMapper _mapper;

    public ProductImageRepository(DbShopContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Delete(int id)
    {
        var found = _context.ProductImages.Find(id);

        if (found is null)
            throw new ArgumentNullException($"ProductImage with id {id} is not found.");

        _context.ProductImages.Remove(found);
        _context.SaveChanges();
    }

    public IEnumerable<ImageEntityDTO> GetAll()
    {
        return _context.ProductImages.ProjectToList<ImageEntityDTO>();
    }

    public ImageEntityDTO? Get(int id)
    {
        // FIXME: Repository returns ImageEntityDTO with filePath
        // Do not include that data to response

        var found = _context.ProductImages
            .Where(p => p.Id == id)
            .AsNoTracking()
            .FirstOrDefault();

        if (found is null)
            return null;

        return _mapper.Map<ImagePrivateEntityDTO>(found);
    }

    public ImageEntityDTO Insert(ImageEntityDTO entity)
    {
        var privateEntity = entity as ImagePrivateEntityDTO;

        if (privateEntity is null)
            throw new Exception("ImagePrivateEntityDTO expected.");

        var insertEntity = _mapper.Map<ImagePrivateEntityDTO, ImageEntity>(privateEntity);

        _context.ProductImages.Add(insertEntity);
        _context.SaveChanges();

        return _mapper.Map<ImageEntityDTO>(insertEntity);
    }

    public ImageEntityDTO? Update(ImageEntityDTO entity)
    {
        var privateEntity = entity as ImagePrivateEntityDTO;

        if (privateEntity is null)
            throw new Exception("ImagePrivateEntityDTO expected.");

        var updateEntity = _mapper.Map<ImagePrivateEntityDTO, ImageEntity>(privateEntity);

        var found = _context.ProductImages.Find(updateEntity.Id);

        if (found is null)
            return null;

        _context.ProductImages.Entry(found).CurrentValues.SetValues(updateEntity);
        _context.SaveChanges();

        return _mapper.Map<ImageEntityDTO>(updateEntity);
    }
}
