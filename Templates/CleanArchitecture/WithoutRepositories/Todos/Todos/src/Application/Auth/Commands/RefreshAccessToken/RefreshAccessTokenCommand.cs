using Todos.Application.Auth.DTOs;

namespace Todos.Application.Auth.Commands.RefreshAccessToken;

public record RefreshAccessTokenCommand(string RefreshToken) : IRequest<RefreshedAccessTokenDto>;
