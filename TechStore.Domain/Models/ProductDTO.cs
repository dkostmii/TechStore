namespace TechStore.Domain.Models;

public class ProductDTO : BaseEntityDTO
{
    public string Name { get; set; }
    public string Description { get; set; }

    public string Model { get; set; }

    public decimal UnitPrice { get; set; }
    public int UnitsAvailable { get; set; }

    public string ProducingCountry { get; set; }

    public IList<ImageEntityDTO> Images { get; set; } = new List<ImageEntityDTO>();
    public CompanyDTO Supplier { get; set; }
    public IList<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
}
