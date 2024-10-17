using Domain.Abstraction;
using Domain.Configuration;
using Domain.Dto;
using Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Implementation.Service;

public class ResultErrorLogService(
    IHttpContextAccessor contextAccessor,
    ILogger<ResultErrorLogService> logger) : IResultErrorLogService
{
    public void Log(IResultInformation result)
    {
        if (result.IsSuccess)
        {
            return;
        }

        var error = result.Error!;
        var traceId = this.GetTraceId() ?? Guid.NewGuid().ToString();
        if (error.InnerException is null)
        {
            logger.LogInformation(
                "ResultError [{TraceId}] |{Type}| {Description}",
                traceId,
                Enum.GetName(error.Type) ?? "unknown",
                error.Description);

            return;
        }
        
        logger.LogCritical(
            error.InnerException,
            "ResultError [{TraceId}] |{Type}| {Description}",
            traceId,
            Enum.GetName(error.Type) ?? "unknown",
            error.Description);
            
        return;
    }

    public ServiceResponse<T> ToServiceResponse<T>(ResultError error)
    {
        var errString = Enum.GetName(error.Type) ?? "unknown";
        return new ServiceResponse<T>(errString);
    }

    private string? GetTraceId()
    {
        return contextAccessor.HttpContext?.Response.Headers
            .TryGetValue(ApplicationConstants.TraceIdHeaderName, out var traceIdValues) == true
                ? traceIdValues.FirstOrDefault()
                : default;
    }
}