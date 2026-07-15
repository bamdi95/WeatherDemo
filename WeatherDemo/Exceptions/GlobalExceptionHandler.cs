using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WeatherDemo.Exceptions;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var isDevelopment = httpContext.RequestServices
            .GetRequiredService<IHostEnvironment>()
            .IsDevelopment();

        var (status, title) = exception switch
        {
            InvalidDataException => (StatusCodes.Status502BadGateway, "Bad Gateway"),
            HttpRequestException => (StatusCodes.Status502BadGateway, "Bad Gateway"),
            OperationCanceledException => (StatusCodes.Status504GatewayTimeout, "Gateway Timeout"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = isDevelopment ? exception.GetType().Name : null,
            Detail = isDevelopment ? exception.Message : null,
            Instance = httpContext.Request.Path
        };


        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
        });
    }
}