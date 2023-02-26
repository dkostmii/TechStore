namespace TechStore.Domain.Types;

public class UserRole
{
    public static readonly UserRole Client = new("Client");
    public static readonly UserRole Admin = new("Admin");

    private readonly string _role;

    private UserRole(string role)
    {
        _role = role;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        var anotherRole = obj as UserRole;

        if (anotherRole is null)
            return false;

        return anotherRole._role == _role;
    }

    public override int GetHashCode()
    {
        return _role.GetHashCode();
    }

    public override string ToString()
    {
        return _role;
    }
}
