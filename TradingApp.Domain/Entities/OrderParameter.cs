namespace TradingApp.Domain.Entities
{
    public class OrderParameter
    {
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? StopPrice { get; set; }
        public OrderType OrderType { get; set; }
        public OrderParameterType OrderParameterType { get; set; }
    }
}