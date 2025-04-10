using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoList.Events;

namespace Todos.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateTodoListCommand, TodoListDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TodoListDto> Handle(
        UpdateTodoListCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoList = await _context
            .TodoLists.Where(l => l.Id == command.Id && l.CreatedBy == _currentUser.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.Id, todoList);

        command.Adapt(todoList);

        todoList.AddDomainEvent(new TodoListUpdatedEvent(todoList));

        await _context.SaveChangesAsync(cancellationToken);

        return todoList.Adapt<TodoListDto>();
    }
}
