namespace TradingApp.Application.Repositories
{
    public interface IWriteRepository<T> where T : class 
    {
        Task<T> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}