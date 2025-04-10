namespace Todos.Application.Shared.Interfaces;

public interface IAppSettings
{
    string BaseUrl { get; }

    string ClientUrl { get; }
}
