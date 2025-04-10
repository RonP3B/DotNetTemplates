using Todos.Application.TodoLists.DTOs;
using Todos.Domain.TodoItem.Enums;

namespace Todos.Application.TodoLists.Queries.GetTodos;

public class GetTodosQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetTodosQuery, TodoListsWithPriorityDto>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<TodoListsWithPriorityDto> Handle(
        GetTodosQuery query,
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
                .TodoLists.AsNoTracking()
                .ProjectToType<TodoListDto>()
                .OrderBy(t => t.Title)
                .ToListAsync(cancellationToken),
        };
    }
}
