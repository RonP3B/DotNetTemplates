using Todos.Application.TodoItems.DTOs;

namespace Todos.Application.TodoItems.Commands.UpdateTodoItemDetail;

public class UpdateTodoItemDetailCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser
) : IRequestHandler<UpdateTodoItemDetailCommand, TodoItemDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TodoItemDto> Handle(
        UpdateTodoItemDetailCommand command,
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
