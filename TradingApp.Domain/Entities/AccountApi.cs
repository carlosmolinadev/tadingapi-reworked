namespace TradingApp.Domain.Entities
{
    public class AccountApi
    {
        public string PrivateKey { get; private set; }
        public string PublicKey { get; private set; }
        public int ExchangeId { get; private set; }
    }
}