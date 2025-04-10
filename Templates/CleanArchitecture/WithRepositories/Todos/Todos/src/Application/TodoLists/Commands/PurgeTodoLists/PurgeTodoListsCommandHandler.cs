using Todos.Domain.TodoList;

namespace Todos.Application.TodoLists.Commands.PurgeTodoLists;

public class PurgeTodoListsCommandHandler(
    ITodoListRepository todoListRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(PurgeTodoListsCommand command, CancellationToken cancellationToken)
    {
        _todoListRepository.Purge();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
