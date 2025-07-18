using System.Text.Json;
using ApplicationException = RevenueRecognition.Application.Exceptions.ApplicationException;

namespace RevenueRecognition.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApplicationException e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = e.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = $"https://httpstatuses.com/{e.StatusCode}",
                title = e.Message,
                status = e.StatusCode
            }));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                type = "https://httpstatuses.com/500",
                title = "Unexpected error occurred.",
                status = 500
            }));
        }
    }
}