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
        var user = await repository.GetUserByEmailAsync(request.Email, token);
        if (user is null) return new HandlerResponse("User not found", 404);

        var isVerified = service.VerifyHashedPassword(user.Password, request.Password);
        if (!isVerified) return new HandlerResponse("Password does not match", 404);
        await unitOfWork.Commit(token);

        var userDTO = mapper.Map<UserResponseDTO>(user);
        return new HandlerResponse("User authenticated", 200, userDTO);
    }
}