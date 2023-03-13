using TechStore.Domain.Interfaces;
using TechStore.Domain.Models;
using TechStore.DAL.Models;
using AutoMapper;

namespace TechStore.DAL.Repos;

public class ProductRepository : IDataRepository<ProductDTO>
{
    private readonly DbShopContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(DbShopContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Delete(int id)
    {
        var found = _context.Products.Find(id);

        if (found is null)
            throw new ArgumentNullException($"Product with id {id} is not found.");

        _context.Products.Remove(found);
        _context.SaveChanges();
    }

    public IEnumerable<ProductDTO> GetAll()
    {
        return _context.Products.ProjectToList<ProductDTO>();
    }

    public ProductDTO? Get(int id)
    {
        var found = _context.Products.Find(id);

        if (found is null)
            return null;

        return _mapper.Map<ProductDTO>(found);
    }

    public ProductDTO Insert(ProductDTO entity)
    {
        var insertEntity = _mapper.Map<ProductDTO, Product>(entity);

        _context.Products.Add(insertEntity);
        _context.SaveChanges();

        return _mapper.Map<ProductDTO>(insertEntity);
    }

    public ProductDTO? Update(ProductDTO entity)
    {
        var updateEntity = _mapper.Map<ProductDTO, Product>(entity);

        var found = _context.Products.Find(updateEntity.Id);

        if (found is null)
            return null;

        _context.Products.Entry(found).CurrentValues.SetValues(updateEntity);
        _context.SaveChanges();

        return _mapper.Map<ProductDTO>(updateEntity);
    }
}

