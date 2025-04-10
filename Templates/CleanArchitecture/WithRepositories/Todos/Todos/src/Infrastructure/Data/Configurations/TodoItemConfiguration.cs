using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoItem.ValueObjects;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Infrastructure.Data.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItemEntity>
{
    public void Configure(EntityTypeBuilder<TodoItemEntity> builder)
    {
        builder
            .Property(b => b.Id)
            .HasConversion(id => id.Value, value => TodoItemId.From(value))
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.ListId)
            .HasConversion(id => id.Value, value => TodoListId.From(value));

        builder.HasKey(b => b.Id);

        builder.OwnsOne(b => b.Note); // EF Core does not yet support nullable complex properties, so this must be owned.
    }
}
