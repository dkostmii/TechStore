using TechStore.Domain.Models;

namespace TechStore.Domain.Interfaces;

public interface IOrderRespository : IDataRepository<OrderDTO>
{
    IEnumerable<OrderDTO> GetAllWithClients();
    IEnumerable<OrderDTO> GetByClient(ClientDTO client);
    IEnumerable<OrderDTO> GetByProduct(ProductDTO product);
}
