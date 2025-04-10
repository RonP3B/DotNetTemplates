using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.Events;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandHandler(
    ITodoListRepository todoListRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : IRequestHandler<UpdateTodoListCommand, TodoListDto>
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TodoListDto> Handle(
        UpdateTodoListCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoList = await _todoListRepository.GetByCreatorAsync(
            TodoListId.From(command.Id),
            _currentUser.Id ?? string.Empty,
            cancellationToken
        );

        Guard.Against.NotFound(command.Id, todoList);

        command.Adapt(todoList);

        todoList.AddDomainEvent(new TodoListUpdatedEvent(todoList));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return todoList.Adapt<TodoListDto>();
    }
}
