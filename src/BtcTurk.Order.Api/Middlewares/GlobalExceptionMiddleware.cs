using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using BtcTurk.Order.Api.Exceptions;

namespace BtcTurk.Order.Api.Middlewares;

/// <summary>
/// This class contains implementation for unexpected or business exception handling
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;

    public GlobalExceptionMiddleware(RequestDelegate next,IHostEnvironment hostEnvironment)
    {
        _next = next;
        _hostEnvironment = hostEnvironment;
    }
    
    /// <summary>
    /// OnInvoke Async
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="logger"></param>
    public async Task InvokeAsync(HttpContext httpContext, ILogger<GlobalExceptionMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {   
            logger.LogError(ex, "Error on API");
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        Activity.Current?.SetStatus(ActivityStatusCode.Error);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var resultMessage = new ExceptionMessage
        {
            Message = GetExceptionMessage(exception),
            TraceId = Activity.Current?.TraceId.ToString()
        };
        if (exception is BusinessException businessException)
        {
            resultMessage.Code = businessException.Code;
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        var result = JsonSerializer.Serialize(resultMessage);
        await httpContext.Response.WriteAsync(result);
    }
    
    private string GetExceptionMessage(Exception exception)
    {
        if (exception is null) return "";
        if (_hostEnvironment.IsProduction())
        {
            return exception.Message;
        }
        StringBuilder stringBuilder = new();
        stringBuilder.Append(exception.Message);
        var ex = exception.InnerException;
        while (ex != null)
        {
            stringBuilder.Append(ex.Message);
            ex = ex.InnerException;
        }
        return stringBuilder.ToString();
    }
}