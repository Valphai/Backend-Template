using Project.Domain.Entities;

namespace Project.Domain.Security;

public sealed class BlockedAttempt : EntityBase
{
    public string IpAddress { get; set; } = string.Empty;
    public DateTime BlockedUntil { get; set; }
}