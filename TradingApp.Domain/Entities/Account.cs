using TradingApp.Domain.Common;

namespace TradingApp.Domain.Entities
{
    public class Account : Entity<Guid>
    {
        public decimal Balance { get; set; }
        public decimal RiskPerTrade { get; set; }
        public Guid UserId { get; set; }
        public int ExchangeId { get; set; }
        public int DerivateId { get; private set; }

        public decimal GetAccountQuantityPerTrade(decimal openPrice, int formatQuantityDigits)
        {
            var quantity = decimal.Round(Balance * (RiskPerTrade / 100) / openPrice, formatQuantityDigits);

            if (quantity == decimal.Zero)
            {
                throw new Exception("Quantity cannot be zero, check on your account settings");
            }

            return quantity;
        }

        public decimal GetBalanceRiskedAmount()
        {
            if (RiskPerTrade < 0 || RiskPerTrade > 99)
            {
                throw new ArgumentException("Risk per trade cannot be lower then 0% or higher than 99%");
            }
            return decimal.Round(Balance * (RiskPerTrade / 100), 2);
        }
    }
}
