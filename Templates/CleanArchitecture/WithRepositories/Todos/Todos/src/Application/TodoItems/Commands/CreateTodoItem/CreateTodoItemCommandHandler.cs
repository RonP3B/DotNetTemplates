using Todos.Application.TodoItems.DTOs;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoItem.Events;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler(
    ICurrentUser currentUser,
    ITodoItemRepository todoItemRepository,
    ITodoListRepository todoListRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly ITodoItemRepository _todoItemRepository = todoItemRepository;
    private readonly ITodoListRepository _todoListRepository = todoListRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TodoItemDto> Handle(
        CreateTodoItemCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoList = await _todoListRepository.GetForReadOnlyAsync(
            TodoListId.From(command.ListId),
            cancellationToken
        );

        Guard.Against.NotFound(command.ListId, todoList);

        if (_currentUser.Id != todoList.CreatedBy)
        {
            throw new ForbiddenAccessException();
        }

        TodoItemEntity todoItem = command.Adapt<TodoItemEntity>();

        todoItem.AddDomainEvent(new TodoItemCreatedEvent(todoItem));

        _todoItemRepository.Add(todoItem);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return todoItem.Adapt<TodoItemDto>();
    }
}
