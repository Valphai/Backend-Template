using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Domain.Security;
using Project.Domain.Services;
using Project.Infrastructure.Config;

namespace Project.Infrastructure.Repositories;

public sealed class SecurityRepository(AppDbContext context) : RepositoryBase<Role>(context), ISecurityRepository
{
    public async Task<bool> IsIpBlockedAsync(string ipAddress, CancellationToken token) => await context.BlockedAttempts
        .AnyAsync(b => b.IpAddress == ipAddress && b.BlockedUntil > DateTime.UtcNow, token);

    public async Task<int> CountFailedAttemptsAsync(string ipAddress, DateTime since, CancellationToken token) => 
        await context.LoginAttempts
            .Where(a => a.IpAddress == ipAddress && !a.Success && a.DateCreated >= since)
            .CountAsync(token);

    public async Task BlockIpAsync(string ipAddress, DateTime until, CancellationToken token)
    {
        var existingBlock = await context.BlockedAttempts
            .FirstOrDefaultAsync(b => b.IpAddress == ipAddress, token);

        if (existingBlock != null)
        {
            existingBlock.BlockedUntil = until;
        }
        else
        {
            await context.BlockedAttempts.AddAsync(new BlockedAttempt
            {
                IpAddress = ipAddress,
                BlockedUntil = until
            }, token);
        }
    }

}