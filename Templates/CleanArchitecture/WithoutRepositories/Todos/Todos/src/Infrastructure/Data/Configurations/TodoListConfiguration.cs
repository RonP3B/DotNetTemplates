using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todos.Domain.TodoList;

namespace Todos.Infrastructure.Data.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoListEntity>
{
    public void Configure(EntityTypeBuilder<TodoListEntity> builder)
    {
        builder.ComplexProperty(b => b.Colour);

        builder.OwnsOne(b => b.ImageKey); // EF Core does not yet support nullable complex properties, so this must be owned.
    }
}
