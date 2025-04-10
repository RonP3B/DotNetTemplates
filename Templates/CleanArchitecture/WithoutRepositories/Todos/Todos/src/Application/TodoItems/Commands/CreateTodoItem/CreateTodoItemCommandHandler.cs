using Todos.Application.TodoItems.DTOs;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoItem.Events;

namespace Todos.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<TodoItemDto> Handle(
        CreateTodoItemCommand command,
        CancellationToken cancellationToken
    )
    {
        var todoList = await _context
            .TodoLists.Where(t => t.Id == command.ListId)
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(command.ListId, todoList);

        if (_currentUser.Id != todoList.CreatedBy)
        {
            throw new ForbiddenAccessException();
        }

        TodoItemEntity todoItem = command.Adapt<TodoItemEntity>();

        todoItem.AddDomainEvent(new TodoItemCreatedEvent(todoItem));

        _context.TodoItems.Add(todoItem);

        await _context.SaveChangesAsync(cancellationToken);

        return todoItem.Adapt<TodoItemDto>();
    }
}
