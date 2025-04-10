using System.Diagnostics;
using Microsoft.Extensions.Logging;

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

        if (elapsedMilliseconds > 500)
        {
            string requestName = typeof(TRequest).Name;
            string userId = _currentUser.Id ?? string.Empty;
            string userName = _currentUser.UserName ?? string.Empty;

            _logger.LogWarning(
                "\nTodos Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}\n",
                requestName,
                elapsedMilliseconds,
                userId,
                userName,
                request
            );
        }

        return response;
    }
}
