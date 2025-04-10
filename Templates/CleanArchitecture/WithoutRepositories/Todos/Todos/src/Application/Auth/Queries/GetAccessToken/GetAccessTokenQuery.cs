using Todos.Application.Auth.DTOs;

namespace Todos.Application.Auth.Queries.GetAccessToken;

public record GetAccessTokenQuery(string RefreshToken) : IRequest<RefreshedAccessTokenDto>;
