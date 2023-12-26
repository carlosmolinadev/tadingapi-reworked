namespace TradingApp.Domain.Entities
{
    public enum OrderStatus
    {
        None,
        New,
        PartiallyFilled,
        Filled,
        Canceled,
        PendingCancel,
        Rejected,
        Expired
    }
}