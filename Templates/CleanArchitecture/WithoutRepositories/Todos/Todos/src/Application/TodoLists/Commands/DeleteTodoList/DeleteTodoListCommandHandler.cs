namespace Todos.Application.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task Handle(DeleteTodoListCommand command, CancellationToken cancellationToken)
    {
        var todoList = await _context
            .TodoLists.Where(l => l.Id == command.Id && l.CreatedBy == _currentUser.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.Id, todoList);

        _context.TodoLists.Remove(todoList);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
