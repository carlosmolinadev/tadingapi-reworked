namespace TradingApp.Domain.Entities
{
    public class Symbol
    {
        public int Id { get; private set; }
        public string Value { get; private set; }
        public int PriceDecimalDigit { get; private set; }
        public int QuantityDecimalDigit { get; private set; }

        public decimal FormatPrice(decimal price)
        {
            return decimal.Round(price, PriceDecimalDigit);
        }

        public decimal FormatQuantity(decimal quantity){
            return decimal.Round(quantity, QuantityDecimalDigit);
        }
    }
}