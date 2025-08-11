namespace PethouseAPI.Services;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    
    Task<T> GetByIdAsync(int id);
    
    Task AddAsync(T entity);
    
    Task UpdateAsync(T entity);
    
    Task DeleteAsync(int id);
}