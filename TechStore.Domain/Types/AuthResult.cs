namespace TechStore.Domain.Types;

public class AuthResult
{
    public string User { get; set; }
    public AuthToken Token { get; set; }
}
