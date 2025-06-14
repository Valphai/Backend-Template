﻿using AutoMapper;
using MediatR;
using Project.Application.DataTransferObjects;
using Project.Application.Interfaces;
using Project.Application.Shared;
using Project.Domain.Interfaces;
using Project.Domain.Providers;
using Project.Domain.Security;
using Project.Domain.Services;

namespace Project.Application.Users.Authentication;

public class AuthenticationHandler(
    Configuration configuration,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPasswordHashingService passwordService,
    ICurrentRequestProvider currentRequestProvider,
    ISecurityRepository securityRepository) : IRequestHandler<AuthenticationRequest, HandlerResponse>
{
    public async Task<HandlerResponse> Handle(AuthenticationRequest request, CancellationToken token)
    {
        var ipAddress = currentRequestProvider.IpAddress;
        if (string.IsNullOrEmpty(ipAddress))
            return new HandlerResponse("Unknown Ip.", 429);

        if (await securityRepository.IsIpBlockedAsync(ipAddress, token))
            return new HandlerResponse("Too many failed attempts. Try again later.", 429);

        var user = await userRepository.GetUserByEmailAsync(request.Email, token);
        if (user is null)
        {
            await LogFailedAttemptAndCheckBlockAsync(ipAddress, token);
            return new HandlerResponse("User not found", 404);
        }

        var isVerified = passwordService.VerifyHashedPassword(user.Password, request.Password);
        if (!isVerified)
        {
            await LogFailedAttemptAndCheckBlockAsync(ipAddress, token);
            return new HandlerResponse("Password incorrect", 404);
        }

        await unitOfWork.Commit(token);

        var userDTO = mapper.Map<UserResponseDTO>(user);
        return new HandlerResponse("User authenticated", 200, userDTO);
    }

    private async Task LogFailedAttemptAndCheckBlockAsync(string ipAddress, CancellationToken token)
    {
        await userRepository.AddLoginAttemptAsync(new LoginAttempt(false, ipAddress), token);

        var now = DateTime.UtcNow;
        var minutesBeforeAttemptsUnblocked = configuration.LoginAttempts.MinutesBeforeAttemptsUnblocked;

        // Count recent failed attempts from this IP in the last M minutes
        var attempts = await securityRepository.CountFailedAttemptsAsync(ipAddress,
            now.AddMinutes(-minutesBeforeAttemptsUnblocked), token);

        // Block IP for M minutes
        if (attempts >= configuration.LoginAttempts.AttemptsBeforeBlockingTheConnection)
            await securityRepository.BlockIpAsync(ipAddress, now.AddMinutes(minutesBeforeAttemptsUnblocked), token);
        
        await unitOfWork.Commit(token);
    }
}