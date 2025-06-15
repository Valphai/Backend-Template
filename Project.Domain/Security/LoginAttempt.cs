using Project.Domain.Entities;

namespace Project.Domain.Security;

public sealed class LoginAttempt(bool success, string? ipAddress) : EntityBase
{
    public bool Success { get; init; } = success;
    public string? IpAddress { get; init; } = ipAddress;
}