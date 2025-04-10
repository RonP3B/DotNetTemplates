using Todos.Domain.TodoItem.Events;

namespace Todos.Application.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoItem = await _context
            .TodoItems.Where(i => i.Id == command.Id && i.CreatedBy == _currentUser.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.Id, todoItem);

        _context.TodoItems.Remove(todoItem);

        todoItem.AddDomainEvent(new TodoItemDeletedEvent(todoItem));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
