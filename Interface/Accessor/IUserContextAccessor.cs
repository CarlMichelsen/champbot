using Domain.Abstraction;
using Domain.User;
using Microsoft.AspNetCore.Http;

namespace Interface.Accessor;

public interface IUserContextAccessor
{
    Result<UserContext> GetUserContext(HttpContext? httpContext = default);
}