using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Infrastructure.Config;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasData(
            new Role("Admin") { DateCreated = DateTimeOffset.UtcNow },
            new Role("User") { DateCreated = DateTimeOffset.UtcNow });
    }
}