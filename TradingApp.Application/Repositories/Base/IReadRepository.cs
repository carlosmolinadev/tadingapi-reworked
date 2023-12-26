namespace TradingApp.Application.Repositories.Base
{
    public interface IReadRepository<T> where T : class
    {
        Task<T?> SelectByIdAsync<Tid>(Tid id);
        Task<IEnumerable<T>> SelectAllAsync();
        Task<IEnumerable<T>> SelectByParameterAsync(QueryParameter queryParameter);
    }
}