using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<TodoListsWithPriorityDto>;
