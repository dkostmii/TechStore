namespace TechStore.DAL.Models;

public class Client : UserEntity
{
    public IList<Order> Orders { get; set; } = new List<Order>();
    public IList<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
}
