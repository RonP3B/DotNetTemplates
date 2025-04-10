using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoLists.Queries.GetTodo;

public class GetTodoQueryHandler(ITodoListRepository todoListRepository)
    : IRequestHandler<GetTodoQuery, TodoListDto>
{
    private readonly ITodoListRepository _todoListRepository = todoListRepository;

    public async Task<TodoListDto> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todoList = await _todoListRepository.GetForReadOnlyAsync(
            TodoListId.From(query.Id),
            cancellationToken
        );

        Guard.Against.NotFound(query.Id, todoList);

        return todoList.Adapt<TodoListDto>();
    }
}
