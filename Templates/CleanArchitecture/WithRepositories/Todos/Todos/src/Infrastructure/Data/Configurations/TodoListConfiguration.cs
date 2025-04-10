using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;

namespace Todos.Infrastructure.Data.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoListEntity>
{
    public void Configure(EntityTypeBuilder<TodoListEntity> builder)
    {
        builder
            .Property(b => b.Id)
            .HasConversion(id => id.Value, value => TodoListId.From(value))
            .ValueGeneratedOnAdd();

        builder.HasKey(b => b.Id);

        builder.ComplexProperty(b => b.Colour);

        builder.OwnsOne(b => b.ImageKey); // EF Core does not yet support nullable complex properties, so this must be owned.
    }
}
