namespace Project.Domain.Security;

public sealed class Configuration
{
    public class SecretsConfig
    {
        public string? JwtPrivateKey { get; set; }
    }

    public class LoginAttemptsConfig
    {
        public int AttemptsBeforeBlockingTheConnection { get; set; }
        public int MinutesBeforeAttemptsUnblocked { get; set; }
    }

    public SecretsConfig Secrets { get; set; } = new();
    public LoginAttemptsConfig LoginAttempts { get; set; } = new();
}