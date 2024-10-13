using Interface.Handler;
using Microsoft.AspNetCore.Mvc;

namespace App.Endpoints;

public static class UserDataEndpoints
{
    public static void RegisterUserDataEndpoints(
        this IEndpointRouteBuilder apiGroup)
    {
        var userGroup = apiGroup
            .MapGroup("user")
            .AllowAnonymous()
            .WithTags("User");
        
        userGroup.MapGet(
            string.Empty,
            async ([FromServices] IUserDataHandler handler) => await handler.GetUserData());
    }
}