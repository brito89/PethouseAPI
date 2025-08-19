namespace PethouseAPI.Services;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    IQueryable<T> GetAll();
    
    Task AddAsync(T entity);
    
    Task UpdateAsync(T entity);
    
}