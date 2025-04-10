using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateTodoItemCommand, TodoItemDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TodoItemDto> Handle(
        UpdateTodoItemCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoItem = await _context
            .TodoItems.Where(i => i.Id == command.Id && i.CreatedBy == _currentUser.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.Id, todoItem);

        command.Adapt(todoItem);

        await _context.SaveChangesAsync(cancellationToken);

        return todoItem.Adapt<TodoItemDto>();
    }
}
