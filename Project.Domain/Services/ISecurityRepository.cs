namespace Project.Domain.Services;

public interface ISecurityRepository
{
    Task<bool> IsIpBlockedAsync(string ipAddress, CancellationToken cancellationToken);
    Task<int> CountFailedAttemptsAsync(string ipAddress, DateTime since, CancellationToken cancellationToken);
    Task BlockIpAsync(string ipAddress, DateTime until, CancellationToken cancellationToken);
}