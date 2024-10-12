namespace Domain.Abstraction;

public sealed class ResultError(
    ErrorType type,
    string description,
    System.Exception? innerException = default)
{
    public ErrorType Type => type;

    public string Description => description;
    
    public System.Exception? InnerException => innerException;

    public List<string> FriendlyErrors { get; init; } = [];
}