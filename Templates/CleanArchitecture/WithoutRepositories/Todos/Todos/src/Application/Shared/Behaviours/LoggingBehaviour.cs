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
        string userInfo = !string.IsNullOrEmpty(userId)
            ? $"{_currentUser.UserName ?? "(unnamed)"} (ID: {userId})"
            : "No authenticated user";

        _logger.LogInformation(
            "[Todos Application] Handling Request: '{RequestName}' | User: {UserInfo}",
            requestName,
            userInfo
        );

        return Task.CompletedTask;
    }
}
