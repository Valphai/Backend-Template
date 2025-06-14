using MediatR;
using Project.Application.Shared;
using Project.Application.Interfaces;
using Project.Domain.Entities;
using Project.Domain.Interfaces;

namespace Project.Application.Users.Create;

public class CreateUserHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IPasswordHashingService service) : IRequestHandler<CreateUserRequest, HandlerResponse>
{
    public async Task<HandlerResponse> Handle(CreateUserRequest request, CancellationToken token)
    {
        var isAvailable = await userRepository.AnyAsync(request.Email, token);
        if (isAvailable) return new HandlerResponse("Email already in use", 404);

        var roles = await roleRepository.GetRoles(request.RoleIds);

        var user = new User(request.Email, service.HashPassword(request.Password), roles);

        // Save user in database
        userRepository.Create(user);
        await unitOfWork.Commit(token);

        return new HandlerResponse("User created", 201);
    }
}