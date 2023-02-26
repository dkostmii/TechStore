using TechStore.Domain.Models;
using TechStore.Domain.Types;

namespace TechStore.Domain.Interfaces;

public interface IAuthenticationRepository<T> where T : LoginEntityDTO
{
    AuthResult Login(T entity);
    AuthResult Register (T entity);
}
