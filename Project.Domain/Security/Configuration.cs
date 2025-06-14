namespace Project.Domain.Security;

public sealed class Configuration
{
    public class SecretsConfiguration
    {
        public string? JwtPrivateKey { get; set; }
    }

    public SecretsConfiguration Secrets { get; set; } = new();
}