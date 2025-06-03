using Evently.Api.Middleware;
using Evently.Api.OpenTelemetry;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Infrastructure.EventBus;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;

namespace Evently.Api.Extensions;

public static class ServiceExtensions
{
    internal static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    internal static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
        });

        return services;
    }

    internal static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration,
        string databaseConnectionString,
        string cacheConnectionString)
    {
        var rabbitMqConnectionString = configuration.GetValue<string>("ConnectionStrings:Queue")!;

        services
            .AddApplication([
                Modules.Events.Application.AssemblyReference.Assembly,
                Modules.Users.Application.AssemblyReference.Assembly,
                Modules.Ticketing.Application.AssemblyReference.Assembly,
                Modules.Attendance.Application.AssemblyReference.Assembly,
            ])
            .AddInfrastructure(
                DiagnosticsConfig.ServiceName,
                [
                    EventsModule.ConfigureConsumers(cacheConnectionString),
                    UsersModule.ConfigureConsumers,
                    TicketingModule.ConfigureConsumers,
                    AttendanceModule.ConfigureConsumers,
                ],
                rabbitMqConnectionString,
                databaseConnectionString,
                cacheConnectionString);

        services
            .AddAttendanceModule(configuration)
            .AddUsersModule(configuration)
            .AddTicketingModule(configuration)
            .AddEventsModule(configuration);


        return services;
    }
}