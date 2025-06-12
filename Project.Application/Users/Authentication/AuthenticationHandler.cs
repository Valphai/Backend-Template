using AutoMapper;
using MediatR;
using Project.Application.DataTransferObjects;
using Project.Application.Interfaces;
using Project.Application.Shared;
using Project.Domain.Entities;
using Project.Domain.Interfaces;

namespace Project.Application.Users.Authentication;

public class AuthenticationHandler(
    IUserRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPasswordHashingService service)
    : IRequestHandler<AuthenticationRequest, HandlerResponse>
{
    public async Task<HandlerResponse> Handle(AuthenticationRequest request, CancellationToken token)
    {
        User? user;
        try
        {
            user = await repository.GetUserByEmailAsync(request.Email, token);
            if (user is null) return new HandlerResponse("User not found", 404);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }

        var isVerified = service.VerifyHashedPassword(user.Password, request.Password);
        if (!isVerified) return new HandlerResponse("Password dont match", 404);

        try
        {
            await unitOfWork.Commit(token);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }

        var userDTO = mapper.Map<UserResponseDTO>(user);
        return new HandlerResponse("User authenticated", 200, userDTO);
    }
}