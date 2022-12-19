using BtcTurk.Order.Api.Middlewares;

namespace BtcTurk.Order.Api.Configurations;

/// <summary>
/// This extension class contains exception handler middleware 
/// </summary>
public static class ExceptionHandlerMiddlewareExtensions  
{  
    /// <summary>
    /// Adds exception middlewares to <see cref="IApplicationBuilder"/>
    /// </summary>
    /// <param name="app"></param>
    public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)  
    {  
        app.UseMiddleware<GlobalExceptionMiddleware>();  
    }  
}