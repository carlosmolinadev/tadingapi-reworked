namespace TradingApp.Domain.Entities
{
    public enum OrderType
    {
        None,
        Limit,
        Market,
        Stop,
        StopMarket,
        TakeProfit,
        TakeProfitMarket,
        TrailingStop
    }
}