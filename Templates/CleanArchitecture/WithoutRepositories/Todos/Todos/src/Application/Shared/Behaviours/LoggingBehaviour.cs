using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Todos.Application.Shared.Behaviours;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, ICurrentUser currentUser)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger = logger;
    private readonly ICurrentUser _currentUser = currentUser;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string userId = _currentUser.Id ?? string.Empty;
        string userName = _currentUser.UserName ?? string.Empty;

        _logger.LogInformation(
            "\nTodos Request: {Name} {@UserId} {@UserName} {@Request}\n",
            requestName,
            userId,
            userName,
            request
        );

        return Task.CompletedTask;
    }
}
