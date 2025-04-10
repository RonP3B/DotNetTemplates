using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoList;

namespace Todos.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandHandler(
    ITodoListRepository todoListRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateTodoListCommand, TodoListDto>
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TodoListDto> Handle(
        CreateTodoListCommand command,
        CancellationToken cancellationToken
    )
    {
        TodoListEntity todoList = command.Adapt<TodoListEntity>();

        _todoListRepository.Add(todoList);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return todoList.Adapt<TodoListDto>();
    }
}
