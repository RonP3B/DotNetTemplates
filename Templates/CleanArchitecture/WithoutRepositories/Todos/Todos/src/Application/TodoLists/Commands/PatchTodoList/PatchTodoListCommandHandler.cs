using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoList.Events;

namespace Todos.Application.TodoLists.Commands.PatchTodoList;

public class PatchTodoListCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<PatchTodoListCommand, TodoListDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TodoListDto> Handle(
        PatchTodoListCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoList = await _context
            .TodoLists.Where(l => l.Id == command.Id && l.CreatedBy == _currentUser.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.Id, todoList);

        command.PartialAdapt(todoList);

        todoList.AddDomainEvent(new TodoListUpdatedEvent(todoList));

        await _context.SaveChangesAsync(cancellationToken);

        return todoList.Adapt<TodoListDto>();
    }
}
