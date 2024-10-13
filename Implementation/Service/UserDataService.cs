using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.User;
using Interface.Accessor;
using Interface.Service;

namespace Implementation.Service;

public class UserDataService(
    IUserContextAccessor userContextAccessor) : IUserDataService
{
    public async Task<Result<ServiceResponse<UserDataDto>>> GetUserData()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500));

        var userContextResult = userContextAccessor.GetUserContext();
        if (userContextResult.IsError)
        {
            return new ServiceResponse<UserDataDto>("unauthorized");
        }

        var user = new UserDataDto(User: userContextResult.Unwrap().User);
        return new ServiceResponse<UserDataDto>(user);
    }
}