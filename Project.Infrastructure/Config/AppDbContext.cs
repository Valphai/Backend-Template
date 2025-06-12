using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;

namespace Project.Infrastructure.Config
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder) => 
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}