using Microsoft.AspNetCore.Identity;
using Todos.Application.Shared.Models;

namespace Todos.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static ApplicationResult<T> ToApplicationResult<T>(this IdentityResult result, T value)
    {
        return result.Succeeded
            ? ApplicationResult<T>.Success(value)
            : ApplicationResult<T>.Failure(
                result.Errors.Select(e => new KeyValuePair<string, string>(
                    MapErrorToPropertyName(e.Code),
                    e.Description
                ))
            );
    }

    public static ApplicationResult ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? ApplicationResult.Success()
            : ApplicationResult.Failure(
                result.Errors.Select(e => new KeyValuePair<string, string>(
                    MapErrorToPropertyName(e.Code),
                    e.Description
                ))
            );
    }

    private static string MapErrorToPropertyName(string errorCode)
    {
        return errorCode switch
        {
            string c when c.Contains("Email", StringComparison.OrdinalIgnoreCase) => "Email",
            string c when c.Contains("Password", StringComparison.OrdinalIgnoreCase) => "Password",
            string c when c.Contains("UserName", StringComparison.OrdinalIgnoreCase) => "UserName",
            // Add more if needed
            _ => nameof(IdentityResult),
        };
    }
}
