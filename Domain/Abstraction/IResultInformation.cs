namespace Domain.Abstraction;

public interface IResultInformation
{
    bool IsError { get; }

    bool IsSuccess { get; }

    ResultError? Error { get; }
}