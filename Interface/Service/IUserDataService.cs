using Domain.Abstraction;
using Domain.Dto;
using Domain.Dto.User;

namespace Interface.Service;

public interface IUserDataService
{
    Task<Result<ServiceResponse<UserDataDto>>> GetUserData();
}