using Project.Domain.Entities;
using Project.Domain.Interfaces;
using Project.Infrastructure.Config;

namespace Project.Infrastructure.Repositories;

public class RepositoryBase<T> : IBaseRepository<T> where T : EntityBase
{
    protected readonly AppDbContext context;

    protected RepositoryBase(AppDbContext context)
    {
        this.context = context;
    }

    public void Create(T entity)
    {
        entity.DateCreated = DateTimeOffset.UtcNow;
        context.Add(entity);
    }

    public void Delete(T entity)
    {
        entity.DateDeleted = DateTimeOffset.UtcNow;
        context.Remove(entity);
    }

    public List<T> GetAll() => context.Set<T>().ToList();

    public T? GetBy(Guid id) => context.Set<T>()
        .FirstOrDefault(x => x.Id == id);

    public void Update(T entity)
    {
        entity.DateUpdated = DateTimeOffset.UtcNow;
        context.Update(entity);
    }
}