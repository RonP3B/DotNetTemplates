using Todos.Application.TodoItems.DTOs;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoItem.ValueObjects;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItemDetail;

public class UpdateTodoItemDetailCommandHandler(
    ICurrentUser currentUser,
    ITodoItemRepository todoItemRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateTodoItemDetailCommand, TodoItemDto>
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ITodoItemRepository _todoItemRepository = todoItemRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TodoItemDto> Handle(
        UpdateTodoItemDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoItem = await _todoItemRepository.GetByCreatorAsync(
            TodoItemId.From(command.Id),
            _currentUser.Id ?? string.Empty,
            cancellationToken
        );

        Guard.Against.NotFound(command.Id, todoItem);

        command.Adapt(todoItem);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return todoItem.Adapt<TodoItemDto>();
    }
}
