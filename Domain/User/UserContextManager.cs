using System.Text.Json;
using Domain.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Domain.User;

public static class UserContextManager
{
    public static Result<UserContext> GetUserContext(HttpContext httpContext)
    {
        try
        {
            if (httpContext.User.Identity?.IsAuthenticated != true)
            {
                return new ResultError(
                    ErrorType.Unauthorized,
                    "Not authorized when accessing user");
            }

            var userJsonClaimResult = GetClaim(httpContext, JwtClaims.User);
            if (userJsonClaimResult.IsError)
            {
                return userJsonClaimResult.Error!;
            }

            var user = JsonSerializer.Deserialize<AuthenticatedUser>(userJsonClaimResult.Unwrap());
            if (user is null)
            {
                return new ResultError(
                    ErrorType.MapError,
                    "User json resulted in a null object");
            }
            
            var accessIdClaimResult = GetClaim(httpContext, JwtClaims.AccessId);
            if (accessIdClaimResult.IsError)
            {
                return accessIdClaimResult.Error!;
            }

            if (!long.TryParse(accessIdClaimResult.Unwrap(), out var accessId))
            {
                return new ResultError(ErrorType.MapError, "Failed to map claim to long (accessId)");
            }
            
            var refreshIdClaimResult = GetClaim(httpContext, JwtClaims.RefreshId);
            if (refreshIdClaimResult.IsError)
            {
                return refreshIdClaimResult.Error!;
            }
            
            if (!long.TryParse(refreshIdClaimResult.Unwrap(), out var refreshId))
            {
                return new ResultError(ErrorType.MapError, "Failed to map claim to long (refreshId)");
            }
            
            var loginIdClaimResult = GetClaim(httpContext, JwtClaims.LoginId);
            if (loginIdClaimResult.IsError)
            {
                return loginIdClaimResult.Error!;
            }
            
            if (!long.TryParse(loginIdClaimResult.Unwrap(), out var loginId))
            {
                return new ResultError(ErrorType.MapError, "Failed to map claim to long (loginId)");
            }

            return new UserContext(
                LoginId: loginId,
                RefreshId: refreshId,
                AccessId: accessId,
                User: user);
        }
        catch (Exception e)
        {
            return new ResultError(
                ErrorType.Exception,
                "Getting user from http-context resulted in an exception",
                e);
        }
    }

    private static Result<string> GetClaim(HttpContext httpContext, string claimType)
    {
        var claim = httpContext.User.Claims
            .FirstOrDefault(c => c.Type == claimType)?.Value;
        if (string.IsNullOrWhiteSpace(claim))
        {
            return new ResultError(
                ErrorType.NotFound,
                $"Did not find \"{claimType}\" claim type");
        }

        return claim;
    }
}