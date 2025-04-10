using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoItem.Enums;
using Todos.Domain.TodoList;

namespace Todos.Application.TodoLists.Queries.GetUserTodos;

public class GetUserTodosQueryHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<GetUserTodosQuery, TodoListsWithPriorityDto>
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;

    public async Task<TodoListsWithPriorityDto> Handle(
        GetUserTodosQuery query,
        CancellationToken cancellationToken
    )
    {
        List<TodoListEntity> todoLists = await _todoListRepository.GetAllByCreatorAsync(
            query.UserId,
            cancellationToken
        );

        return new TodoListsWithPriorityDto
        {
            PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                .Cast<PriorityLevel>()
                .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() })
                .ToList(),

            Lists = todoLists.Adapt<IReadOnlyCollection<TodoListDto>>(),
        };
    }
}
