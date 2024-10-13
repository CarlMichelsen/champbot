using Interface.Handler;
using Interface.Service;
using Microsoft.AspNetCore.Http;

namespace Implementation.Handler;

public class UserDataHandler(
    IResultErrorLogService resultErrorLogService,
    IUserDataService userDataService) : IUserDataHandler
{
    public async Task<IResult> GetUserData()
    {
        var userDataResult = await userDataService.GetUserData();
        if (userDataResult.IsError)
        {
            resultErrorLogService.Log(userDataResult);
            return Results.StatusCode(500);
        }

        return Results.Ok(userDataResult.Unwrap());
    }
}