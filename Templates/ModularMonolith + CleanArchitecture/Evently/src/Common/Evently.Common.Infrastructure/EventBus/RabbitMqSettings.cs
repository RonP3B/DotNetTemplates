namespace Evently.Common.Infrastructure.EventBus;

public sealed record RabbitMqSettings
{
    public string Host { get; set; } = null!;
}
