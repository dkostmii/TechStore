namespace TechStore.Domain.Models;

public class ClientDTO : UserEntityDTO
{
    public IList<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
    public IList<ShippingAddressDTO> ShippingAddresses { get; set; } = new List<ShippingAddressDTO>();
}
