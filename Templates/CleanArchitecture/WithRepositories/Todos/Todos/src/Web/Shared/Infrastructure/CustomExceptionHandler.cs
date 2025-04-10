using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Shared.Exceptions;
using Todos.Domain.Shared.Exceptions;

namespace Todos.Web.Shared.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;
    private readonly IWebHostEnvironment _environment;

    public CustomExceptionHandler(IWebHostEnvironment environment)
    {
        _environment = environment;

        // Register needed exception types and handlers.
        _exceptionHandlers = new()
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(BusinessRuleException), HandleBusinessRuleException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(BadHttpRequestException), HandleBadHttpRequestException },
        };
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var currentExceptionType = exception.GetType();

        // Traverse up the exception's inheritance chain to find a registered handler
        while (currentExceptionType != null)
        {
            if (_exceptionHandlers.TryGetValue(currentExceptionType, out var handler))
            {
                await handler(httpContext, exception);
                return true;
            }

            currentExceptionType = currentExceptionType.BaseType;
        }

        // In development, ASP.NET Core will generate a detailed response for the unhandled exception
        // Developers can see the stack trace and additional details for debugging
        if (_environment.IsDevelopment())
        {
            return false;
        }

        // In others enviroments, hide sensitive details and return a generic error response
        await HandleUnknownException(httpContext);

        return true;
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception ex)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            new ValidationProblemDetails(exception.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            }
        );
    }

    private async Task HandleBusinessRuleException(HttpContext httpContext, Exception ex)
    {
        var businessException = (BusinessRuleException)ex;

        var validationError = new Dictionary<string, string[]>
        {
            { businessException.PropertyName, new[] { businessException.Message } },
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            new ValidationProblemDetails(validationError)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            }
        );
    }

    private async Task HandleNotFoundException(HttpContext httpContext, Exception ex)
    {
        var exception = (NotFoundException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message,
            }
        );
    }

    private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
    {
        var unauthorizedException = (UnauthorizedAccessException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Detail = unauthorizedException.Message,
            }
        );
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        var forbiddenException = (ForbiddenAccessException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Detail = forbiddenException.Message,
            }
        );
    }

    private async Task HandleBadHttpRequestException(HttpContext httpContext, Exception ex)
    {
        var badRequestException = (BadHttpRequestException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = badRequestException.Message,
            }
        );
    }

    private static async Task HandleUnknownException(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected error",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            }
        );
    }
}
