using AutoMapper;
using MediatR;
using Project.Application.DataTransferObjects;
using Project.Application.Shared;
using Project.Domain.Entities;
using Project.Domain.Interfaces;

namespace Project.Application.Users.RefreshToken;

public class RefreshTokenHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<RefreshTokenRequest, HandlerResponse>
{
    public async Task<HandlerResponse> Handle(RefreshTokenRequest request, CancellationToken token)
    {
        User? user;
        try
        {
            user = await userRepository.GetUserByRefreshCode(request.RefreshToken, token);
            if (user is null) return new HandlerResponse("User not found", 404);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }

        // Update Refresh token
        user.GenerateRefreshToken();

        try
        {
            await unitOfWork.Commit(token);
        }
        catch
        {
            return new HandlerResponse("Internal Server Error", 500);
        }

        var userDTO = mapper.Map<UserResponseDTO>(user);
        return new HandlerResponse("Token Refreshed", 200, userDTO);
    }
}