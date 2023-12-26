namespace TradingApp.Domain.Enums
{
    public enum TradeStatus
    {
        Pending, 
        Finished,
        Active,
        WaitingConfirmation,
        Failed,
        WaitingCandle,
    }
}