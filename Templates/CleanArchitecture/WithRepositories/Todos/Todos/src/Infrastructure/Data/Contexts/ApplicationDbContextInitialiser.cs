using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Todos.Domain.Shared.Constants;
using Todos.Domain.TodoItem;
using Todos.Domain.TodoList;
using Todos.Domain.TodoList.ValueObjects;
using Todos.Infrastructure.Identity;

namespace Todos.Infrastructure.Data.Contexts;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        ApplicationDbContextInitialiser initialiser =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser
        {
            UserName = "administrator@localhost",
            Email = "administrator@localhost",
            EmailConfirmed = true,
        };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            string adminPassword = Guard.Against.NullOrWhiteSpace(
                _configuration["AdminPassword"],
                message: "AdminPassword not configured"
            );

            await _userManager.CreateAsync(administrator, adminPassword);

            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
            }
        }

        // Default data
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(
                new TodoListEntity
                {
                    Title = "Todo List #0",
                    Colour = Colour.White,
                    Items =
                    {
                        new TodoItemEntity
                        {
                            Title = "Make a todo list 📃",
                            CreatedBy = administrator.Id,
                            LastModifiedBy = administrator.Id,
                        },
                        new TodoItemEntity
                        {
                            Title = "Check off the first item ✅",
                            CreatedBy = administrator.Id,
                            LastModifiedBy = administrator.Id,
                        },
                        new TodoItemEntity
                        {
                            Title = "Realise you've already done two things on the list! 🤯",
                            CreatedBy = administrator.Id,
                            LastModifiedBy = administrator.Id,
                        },
                        new TodoItemEntity
                        {
                            Title = "Reward yourself with a nice, long nap 🏆",
                            CreatedBy = administrator.Id,
                            LastModifiedBy = administrator.Id,
                        },
                    },
                    CreatedBy = administrator.Id,
                    LastModifiedBy = administrator.Id,
                }
            );

            await _context.SaveChangesAsync();
        }
    }
}
