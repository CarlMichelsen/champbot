using Domain.Abstraction;
using Domain.User;
using Interface.Accessor;
using Microsoft.AspNetCore.Http;

namespace Implementation.Accessor;

public class UserContextAccessor(
    IHttpContextAccessor httpContextAccessor) : IUserContextAccessor
{
    public Result<UserContext> GetUserContext(HttpContext? httpContext = default)
    {
        var context = httpContextAccessor.HttpContext ?? httpContext;
        if (context is null)
        {
            return new ResultError(
                ErrorType.MapError,
                "No http-context found when getting user");
        }
        
        return UserContextManager.GetUserContext(context);
    }
}