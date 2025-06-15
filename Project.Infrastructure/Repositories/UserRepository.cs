using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using Project.Domain.Security;
using Project.Infrastructure.Config;

namespace Project.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    public Task<bool> AnyAsync(string email, CancellationToken token) => context.Users
        .AnyAsync(x => x.Email == email, token);

    public async Task AddLoginAttemptAsync(LoginAttempt loginAttempt, CancellationToken token) =>
        await context.LoginAttempts.AddAsync(loginAttempt, token);

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken token) => context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == email, token);

    public Task<User?> GetUserByRefreshCode(Guid refreshToken, CancellationToken token) => context.Users
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, token);
}