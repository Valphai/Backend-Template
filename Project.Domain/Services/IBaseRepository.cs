using Project.Domain.Entities;

namespace Project.Domain.Interfaces;

public interface IBaseRepository<T> where T : EntityBase
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    T? GetBy(Guid id);
    List<T> GetAll();
}