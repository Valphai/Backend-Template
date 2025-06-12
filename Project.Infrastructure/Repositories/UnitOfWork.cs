using Project.Domain.Interfaces;
using Project.Infrastructure.Config;

namespace Project.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task Commit(CancellationToken token) => await context.SaveChangesAsync(token);
}