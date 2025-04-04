using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    bool Exists(int id);
    Task<int> CountAsync(ISpecification<T> spec);
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
}
