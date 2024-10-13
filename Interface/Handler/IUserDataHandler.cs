using Microsoft.AspNetCore.Http;

namespace Interface.Handler;

public interface IUserDataHandler
{
    Task<IResult> GetUserData();
}