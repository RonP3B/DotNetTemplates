using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Queries.GetUserTodos;

[Authorize]
public record GetUserTodosQuery(string UserId) : IRequest<TodoListsWithPriorityDto>;
