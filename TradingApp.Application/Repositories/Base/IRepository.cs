namespace TradingApp.Application.Repositories.Base
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class
    {
    }
}