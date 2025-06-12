using MediatR;
using Project.Application.Shared;

namespace Project.Application.Users.RefreshToken;

public record RefreshTokenRequest(Guid RefreshToken) : IRequest<HandlerResponse>;