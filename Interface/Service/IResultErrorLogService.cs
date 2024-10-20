using Domain.Abstraction;

namespace Interface.Service;

public interface IResultErrorLogService
{
    void Log(IResultInformation result);
}