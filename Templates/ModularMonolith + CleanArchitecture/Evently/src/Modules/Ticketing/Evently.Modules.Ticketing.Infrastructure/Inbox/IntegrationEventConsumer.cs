using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Inbox;
using Evently.Common.Infrastructure.Serialization;
using MassTransit;
using Newtonsoft.Json;

namespace Evently.Modules.Ticketing.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>(
    IDbConnectionFactory dbConnectionFactory)
    : IConsumer<TIntegrationEvent> 
    where TIntegrationEvent : IntegrationEvent
{
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        var integrationEvent = context.Message;

        var inboxMessage = new InboxMessage
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonConvert.SerializeObject(integrationEvent, SerializerSettings.Instance),
            OccurredAtUtc = integrationEvent.OccurredAtUtc
        };

        const string sql =
            """
            INSERT INTO ticketing.inbox_messages(id, type, content, occurred_at_utc)
            VALUES (@Id, @Type, @Content::json, @OccurredAtUtc);
            """;
        
        await connection.ExecuteAsync(sql, inboxMessage);
    }
}