namespace Todos.Application.TodoLists.Commands.DeleteTodoList;

[Authorize]
public record DeleteTodoListCommand(int Id) : IRequest;
