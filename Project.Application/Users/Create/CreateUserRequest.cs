using MediatR;
using Project.Application.Shared;

namespace Project.Application.Users.Create;

public record CreateUserRequest(string Email, string Password, List<Guid> RoleIds) : IRequest<HandlerResponse>;