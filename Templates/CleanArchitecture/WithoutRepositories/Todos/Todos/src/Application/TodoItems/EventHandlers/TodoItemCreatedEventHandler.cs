using Microsoft.Extensions.Logging;
using Todos.Domain.TodoItem.Events;

namespace Todos.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger = logger;

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "\nTodos Domain Event: {DomainEvent}\n",
            notification.GetType().Name
        );

        return Task.CompletedTask;
    }
}
