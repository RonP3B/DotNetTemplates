using Microsoft.AspNetCore.Identity;

namespace Todos.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? ImageKey { get; set; }
}
