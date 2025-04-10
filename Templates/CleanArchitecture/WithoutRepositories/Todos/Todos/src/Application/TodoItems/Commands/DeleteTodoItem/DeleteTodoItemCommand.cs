namespace Todos.Application.TodoItems.Commands.DeleteTodoItem;

[Authorize]
public record DeleteTodoItemCommand(int Id) : IRequest;
