using System.Net.Http.Headers;

namespace TechStore.Domain.Models;

public class OrderDTO : BaseEntityDTO
{
    public DateTime OrderedAt { get; set; }
    public DateTime? ReceivedAt { get; set; }

    public ReviewDTO? Review { get; set; }
    public ClientDTO Client { get; set; }
    public IList<ProductDTO> Products { get; set; } = new List<ProductDTO>();
}
