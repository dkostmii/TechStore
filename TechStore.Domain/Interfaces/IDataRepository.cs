using TechStore.Domain.Models;

namespace TechStore.Domain.Interfaces;

public interface IDataRepository<T> where T : BaseEntityDTO
{
    IEnumerable<T> GetAll();
    T? Get(int id);
    T Insert(T entity);
    // FIXME: Accept id parameter here as in controller
    T? Update(T entity);
    void Delete(int id);
}
