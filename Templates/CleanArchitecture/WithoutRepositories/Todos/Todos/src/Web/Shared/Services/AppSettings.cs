using Todos.Application.Shared.Interfaces;

namespace Todos.Web.Shared.Services;

public class AppSettings(IConfiguration configuration) : IAppSettings
{
    private readonly IConfiguration _configuration = configuration;

    public string BaseUrl =>
        Guard.Against.NullOrWhiteSpace(
            _configuration["BaseUrl"],
            message: "BaseUrl not configured"
        );

    public string ClientUrl =>
        Guard.Against.NullOrWhiteSpace(
            _configuration["ClientUrl"],
            message: "ClientUrl not configured"
        );
}
