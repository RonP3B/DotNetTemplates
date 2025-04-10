using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoLists.Commands.DeleteTodoList;

public class DeleteTodoListCommandHandler(
    ITodoListRepository todoListRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : IRequestHandler<DeleteTodoListCommand>
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task Handle(DeleteTodoListCommand command, CancellationToken cancellationToken)
    {
        var todoList = await _todoListRepository.GetByCreatorAsync(
            TodoListId.From(command.Id),
            _currentUser.Id ?? string.Empty,
            cancellationToken
        );

        Guard.Against.NotFound(command.Id, todoList);

        _todoListRepository.Remove(todoList);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
