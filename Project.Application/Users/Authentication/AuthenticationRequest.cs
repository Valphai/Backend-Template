using MediatR;
using Project.Application.Shared;

namespace Project.Application.Users.Authentication;

public record AuthenticationRequest(string Email, string Password) : IRequest<HandlerResponse>;