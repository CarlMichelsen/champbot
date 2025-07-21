using System.Net;

namespace Application.Exceptions;

public class ProblemDetailsException(string message, HttpStatusCode status)
    : System.Exception(message)
{
    public HttpStatusCode Status { get; } = status;
}