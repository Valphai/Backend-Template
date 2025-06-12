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
        try
        {
            // Check if email is avaliable
            var isAvailable = await userRepository.AnyAsync(request.Email, token);
            if (isAvailable) return new HandlerResponse("Email already in use", 404);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }

        // Get roles
        List<Role> roles;
        try
        {
            roles = await roleRepository.GetRoles(request.RoleIds);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }


        // Generate User object
        var user = new User(request.Email, service.HashPassword(request.Password)) {
            Roles = roles
        };

        try
        {
            // Save user in database
            userRepository.Create(user);
            // Commit the chages in database
            await unitOfWork.Commit(token);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }

        return new HandlerResponse("User created", 201);
    }
}