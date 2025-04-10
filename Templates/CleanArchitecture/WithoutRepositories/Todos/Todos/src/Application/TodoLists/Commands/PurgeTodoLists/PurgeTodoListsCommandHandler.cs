namespace Todos.Application.TodoLists.Commands.PurgeTodoLists;

public class PurgeTodoListsCommandHandler(IApplicationDbContext context)
    : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(PurgeTodoListsCommand command, CancellationToken cancellationToken)
    {
        _context.TodoLists.RemoveRange(_context.TodoLists);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
