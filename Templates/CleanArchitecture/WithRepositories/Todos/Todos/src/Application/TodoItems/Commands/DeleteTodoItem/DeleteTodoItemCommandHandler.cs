using Todos.Domain.TodoItem;
using Todos.Domain.TodoItem.Events;
using Todos.Domain.TodoItem.ValueObjects;

namespace Todos.Application.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler(
    ICurrentUser currentUser,
    ITodoItemRepository todoItemRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ITodoItemRepository _todoItemRepository = todoItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await _todoItemRepository.GetByCreatorAsync(
            TodoItemId.From(command.Id),
            _currentUser.Id ?? string.Empty,
            cancellationToken
        );

        Guard.Against.NotFound(command.Id, todoItem);

        _todoItemRepository.Remove(todoItem);

        todoItem.AddDomainEvent(new TodoItemDeletedEvent(todoItem));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
