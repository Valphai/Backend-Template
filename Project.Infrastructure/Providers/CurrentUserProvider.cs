using Microsoft.AspNetCore.Http;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using Project.Domain.Providers;
using Project.Infrastructure.Extensions;

namespace Project.Infrastructure.Providers;

internal sealed class CurrentRequestProvider(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    : ICurrentRequestProvider
{
    public string? IpAddress => httpContextAccessor.HttpContext?
        .Connection.RemoteIpAddress?
        .ToString();

    public User CurrentUser
    {
        get
        {
            var userId = httpContextAccessor
                .HttpContext?
                .User
                .GetUserId();

            if (!userId.HasValue) throw new UnauthorizedAccessException("No current user found");

            return userRepository.GetById(userId.Value) ?? throw new UnauthorizedAccessException("No user found by given Id!");
        }
    }
}