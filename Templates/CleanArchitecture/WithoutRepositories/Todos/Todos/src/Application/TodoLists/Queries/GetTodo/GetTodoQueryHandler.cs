using Todos.Application.TodoLists.DTOs;

namespace Todos.Application.TodoLists.Queries.GetTodo;

public class GetTodoQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTodoQuery, TodoListDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<TodoListDto> Handle(GetTodoQuery query, CancellationToken cancellationToken)
    {
        var todoList = await _context
            .TodoLists.Where(t => t.Id == query.Id)
            .AsNoTracking()
            .ProjectToType<TodoListDto>()
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(query.Id, todoList);

        return todoList;
    }
}
