using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Domain.Interfaces;
using Project.Infrastructure.Config;

namespace Project.Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : RepositoryBase<Role>(context), IRoleRepository
{
    public async Task<List<Role>> GetRoles(List<Guid> ids) => await context.Roles
        .Where(x => ids.Contains(x.Id))
        .ToListAsync();
}