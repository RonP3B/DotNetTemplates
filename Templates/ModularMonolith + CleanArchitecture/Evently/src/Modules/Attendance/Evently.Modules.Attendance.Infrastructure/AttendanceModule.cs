using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Common.Infrastructure.Database;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Infrastructure.Attendees;
using Evently.Modules.Attendance.Infrastructure.Authentication;
using Evently.Modules.Attendance.Infrastructure.Database;
using Evently.Modules.Attendance.Infrastructure.Events;
using Evently.Modules.Attendance.Infrastructure.Inbox;
using Evently.Modules.Attendance.Infrastructure.Outbox;
using Evently.Modules.Attendance.Infrastructure.Tickets;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Modules.Attendance.Infrastructure;

public static class AttendanceModule
{
    public static IServiceCollection AddAttendanceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();
        
        services
            .AddInfrastructure(configuration)
            .AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<IAttendanceContext, AttendanceContext>()
            .AddDatabase(configuration)
            .AddOutbox(configuration)
            .AddInbox(configuration);
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<AttendanceDbContext>(Postgres.StandardOptions(configuration, Schemas.Attendance))
            .AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<AttendanceDbContext>())
            .AddScoped<IAttendeeRepository, AttendeeRepository>()
            .AddScoped<IEventRepository, EventRepository>()
            .AddScoped<ITicketRepository, TicketRepository>();
    
    private static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<OutboxOptions>(configuration.GetSection("Attendance:Outbox"))
            .ConfigureOptions<ConfigureProcessOutboxJob>();
    
    private static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<InboxOptions>(configuration.GetSection("Attendance:Inbox"))
            .ConfigureOptions<ConfigureProcessInboxJob>();
    
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator, string instanceId) =>
        Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToList()
            .ForEach(integrationEventHandlerType =>
            {
                var integrationEventType = integrationEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();
                
                registrationConfigurator
                    .AddConsumer(typeof(IntegrationEventConsumer<>)
                    .MakeGenericType(integrationEventType))
                    .Endpoint(c=> c.InstanceId = instanceId);
            });
    
    private static void AddDomainEventHandlers(this IServiceCollection services) =>
        Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToList()
            .ForEach(domainEventHandlerType =>
            {
                services.TryAddScoped(domainEventHandlerType);

                var domainEventType = domainEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandlerType = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEventType);
                
                services.Decorate(domainEventHandlerType, closedIdempotentHandlerType);
            });
    
    private static void AddIntegrationEventHandlers(this IServiceCollection services) =>
        Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToList()
            .ForEach(integrationEventHandlerType =>
            {
                services.TryAddScoped(integrationEventHandlerType);

                var integrationEventType = integrationEventHandlerType
                    .GetInterfaces()
                    .Single(@interface => @interface.IsGenericType)
                    .GetGenericArguments()
                    .Single();

                var closedIdempotentHandlerType = typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEventType);
                
                services.Decorate(integrationEventHandlerType, closedIdempotentHandlerType);
            });
}