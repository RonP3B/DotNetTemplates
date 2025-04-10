using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todos.Application.Shared.Interfaces;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoList;
using Todos.Infrastructure.Identity;

namespace Todos.Infrastructure.Data.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<TodoListEntity> TodoLists => Set<TodoListEntity>();

    public DbSet<TodoItemEntity> TodoItems => Set<TodoItemEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
