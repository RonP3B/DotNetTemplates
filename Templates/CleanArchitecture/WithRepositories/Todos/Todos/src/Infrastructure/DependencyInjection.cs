using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Todos.Application.Shared.Interfaces;
using Todos.Domain.Shared.Constants;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoList;
using Todos.Infrastructure.Data;
using Todos.Infrastructure.Data.Contexts;
using Todos.Infrastructure.Data.Repositories;
using Todos.Infrastructure.Email;
using Todos.Infrastructure.FileServices;
using Todos.Infrastructure.Identity;
using Todos.Infrastructure.Security.Jwt;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<EmailSettings>(configuration.GetSection("MailSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        string connectionString = Guard.Against.Null(
            configuration.GetConnectionString("DefaultConnection"),
            "Connection string 'DefaultConnection' not configured."
        );

        EmailSettings mailSettings = Guard.Against.Null(
            configuration.GetSection("MailSettings").Get<EmailSettings>(),
            "MailSettings not configured."
        );

        JwtSettings jwtSettings = Guard.Against.Null(
            configuration.GetSection("JwtSettings").Get<JwtSettings>(),
            "JwtSettings not configured."
        );

        services.AddScoped<ITodoListRepository, TodoListRepository>();
        services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileStorageService, ApiFileStorage>();
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddSingleton<ITemplateService, TemplateManager>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            }
        );

        byte[] secretKey = Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecretKey);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services
            .AddAuthorizationBuilder()
            .AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IApplicationUserManager, ApplicationUserManager>();
        services.AddTransient<IAuthService, IdentityAuthService>();

        return services;
    }
}
