namespace Todos.Application.Shared.Interfaces;

public interface ICurrentUser
{
    string? Id { get; }

    string? UserName { get; }
}
