namespace TechStore.Domain.Types;

public class AuthToken
{
    public string Value { get; set; }
    public DateTime ExpiresAt { get; set; }
}
