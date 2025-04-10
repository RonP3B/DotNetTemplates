using Todos.Application.TodoItems.DTOs;
using Todos.Domain.Shared.ResultTypes;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class GetTodoItemsWithPaginationQueryHandler(ITodoItemRepository todoItemRepository)
    : IRequestHandler<GetTodoItemsWithPaginationQuery, PagedResult<TodoItemBriefDto>>
{
    private readonly ITodoItemRepository _todoItemRepository = todoItemRepository;

    public async Task<PagedResult<TodoItemBriefDto>> Handle(
        GetTodoItemsWithPaginationQuery query,
        CancellationToken cancellationToken
    )
    {
        PagedResult<TodoItemEntity> todoItems = await _todoItemRepository.GetPagedByListIdAsync(
            TodoListId.From(query.ListId),
            query.PageNumber,
            query.PageSize,
            cancellationToken
        );

        return todoItems.Adapt<PagedResult<TodoItemBriefDto>>();
    }
}
