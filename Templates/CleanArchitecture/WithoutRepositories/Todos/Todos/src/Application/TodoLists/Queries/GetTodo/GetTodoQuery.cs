using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Queries.GetTodo;

[Authorize]
public record GetTodoQuery(int Id) : IRequest<TodoListDto>;
