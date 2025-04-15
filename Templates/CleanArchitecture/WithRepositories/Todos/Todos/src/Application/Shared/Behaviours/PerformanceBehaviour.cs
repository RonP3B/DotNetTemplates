using System.Diagnostics;

namespace Todos.Application.Shared.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>(
    ILogger<TRequest> logger,
    ICurrentUser currentUser
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new Stopwatch();
    private readonly ILogger<TRequest> _logger = logger;
    private readonly ICurrentUser _currentUser = currentUser;
    private const int WarningThresholdMs = 500;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _timer.Start();

        TResponse response = await next();

        _timer.Stop();

        long elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > WarningThresholdMs)
        {
            string requestName = typeof(TRequest).Name;
            string userId = _currentUser.Id ?? string.Empty;
            string userInfo = !string.IsNullOrEmpty(userId)
                ? $"{_currentUser.UserName ?? "(unnamed)"} (ID: {userId})"
                : "No authenticated user";

            _logger.LogWarning(
                "[Performance Warning] Request '{RequestName}' took {ElapsedMilliseconds}ms to complete. "
                    + "Threshold is {Threshold}ms. User: {UserInfo}",
                requestName,
                elapsedMilliseconds,
                WarningThresholdMs,
                userInfo
            );
        }

        return response;
    }
}
