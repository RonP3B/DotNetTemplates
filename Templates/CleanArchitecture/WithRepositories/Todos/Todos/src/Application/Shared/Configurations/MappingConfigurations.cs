using Todos.Application.TodoItems.Mappings;
using Todos.Application.TodoLists.Mappings;

namespace Todos.Application.Shared.Configurations;

public static class MappingConfigurations
{
    public static void Configure()
    {
        TodoItemsMapping.Configure();
        TodoListsMapping.Configure();
    }
}
