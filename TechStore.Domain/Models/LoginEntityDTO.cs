namespace TechStore.Domain.Models;

public abstract class LoginEntityDTO : BaseEntityDTO
{
    public string Login { get; set; }
    public string Password { get; set; }
}
