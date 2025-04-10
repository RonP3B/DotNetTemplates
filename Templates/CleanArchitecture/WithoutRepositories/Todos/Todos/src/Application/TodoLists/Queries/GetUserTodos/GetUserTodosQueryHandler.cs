using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoItem.Enums;

namespace Todos.Application.TodoLists.Queries.GetUserTodos;

public class GetUserTodosQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetUserTodosQuery, TodoListsWithPriorityDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<TodoListsWithPriorityDto> Handle(
        GetUserTodosQuery query,
        CancellationToken cancellationToken
    )
    {
        return new TodoListsWithPriorityDto
        {
            PriorityLevels = Enum.GetValues(typeof(PriorityLevel))
                .Cast<PriorityLevel>()
                .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() })
                .ToList(),

            Lists = await _context
                .TodoLists.Where(t => t.CreatedBy == query.UserId)
                .AsNoTracking()
                .ProjectToType<TodoListDto>()
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken),
        };
    }
}
