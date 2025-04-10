using Todos.Domain.TodoItem.ValueObjects;

namespace Todos.Application.TodoItems.Mappings;

public static class TodoItemsMapping
{
    public static void Configure()
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

        config.ForType<int, TodoItemId>().ConstructUsing(src => TodoItemId.From(src));
    }
}
