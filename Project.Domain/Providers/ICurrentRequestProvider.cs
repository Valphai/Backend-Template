using Project.Domain.Entities;

namespace Project.Domain.Providers;

public interface ICurrentRequestProvider
{
    string? IpAddress { get; }
    User CurrentUser { get; }
}