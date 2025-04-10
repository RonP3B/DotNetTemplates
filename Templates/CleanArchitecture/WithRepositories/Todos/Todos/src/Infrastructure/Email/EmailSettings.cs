namespace Todos.Infrastructure.Email;

public record EmailSettings
{
    public required string SmtpServer { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }

    public required string FromName { get; init; }

    public int Port { get; init; }
}
