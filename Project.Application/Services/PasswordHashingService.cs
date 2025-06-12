using Project.Application.Interfaces;

namespace Project.Application.Services;

public class PasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string password)
    {
        ArgumentNullException.ThrowIfNull(password);

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);

        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}