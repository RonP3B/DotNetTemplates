using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using Todos.Application.Shared.Interfaces;
using Todos.Infrastructure.Data.Contexts;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddScoped<IAppSettings, AppSettings>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true
        );

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(
            (configure, sp) =>
            {
                configure.Title = "Todos API";

                // Add JWT
                configure.AddSecurity(
                    "JWT",
                    Enumerable.Empty<string>(),
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}.",
                    }
                );

                configure.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT")
                );
            }
        );

        return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        var keyVaultUri = configuration["AZURE_KEY_VAULT_ENDPOINT"];

        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        return services;
    }
}
