using Domain.Abstraction;
using Domain.Dto;

namespace Interface.Service;

public interface IResultErrorLogService
{
    void Log(IResultInformation result);
    
    ServiceResponse<T> ToServiceResponse<T>(ResultError error);
}