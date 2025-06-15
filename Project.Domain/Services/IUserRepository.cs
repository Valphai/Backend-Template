using Project.Domain.Entities;
using Project.Domain.Security;

namespace Project.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email, CancellationToken token);
    public Task<User?> GetUserByRefreshCode(Guid refreshToken, CancellationToken token);
    Task<bool> AnyAsync(string email, CancellationToken token);
    Task AddLoginAttemptAsync(LoginAttempt loginAttempt, CancellationToken token);
}