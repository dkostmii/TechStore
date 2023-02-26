namespace TechStore.Domain.Models;

public abstract class UserEntityDTO : LoginEntityDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
