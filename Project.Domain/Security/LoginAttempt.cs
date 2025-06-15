using Project.Domain.Entities;

namespace Project.Domain.Security;

public sealed class LoginAttempt : EntityBase
{
    public bool Success { get; set; }
    public string? IpAddress { get; set; }
}