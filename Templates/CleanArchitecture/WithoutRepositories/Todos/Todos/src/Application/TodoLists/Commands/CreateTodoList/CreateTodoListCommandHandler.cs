using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoList;

namespace Todos.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateTodoListCommand, TodoListDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<TodoListDto> Handle(
        CreateTodoListCommand command,
        CancellationToken cancellationToken
    )
    {
        TodoListEntity todoList = command.Adapt<TodoListEntity>();

        _context.TodoLists.Add(todoList);

        await _context.SaveChangesAsync(cancellationToken);

        return todoList.Adapt<TodoListDto>();
    }
}
