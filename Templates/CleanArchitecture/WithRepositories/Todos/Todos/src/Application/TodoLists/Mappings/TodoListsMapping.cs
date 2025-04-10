using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Application.TodoLists.Mappings;

public static class TodoListsMapping
{
    public static void Configure()
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

        config.ForType<int, TodoListId>().ConstructUsing(src => TodoListId.From(src));
    }
}
