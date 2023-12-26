using System.ComponentModel.DataAnnotations;
using TradingApp.Domain.Entities;

namespace TradingApp.Application.Models.Requests
{
    public class OrderParameterRequest : IValidatableObject
    {
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal? StopPrice { get; set; }
        public string OrderType { get; set; } = "Limit";
        [Required]
        public required string OrderParameterType { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Enum.TryParse(OrderType, true, out OrderType orderType))
            {
                yield return new ValidationResult("Invalid OrderType", new[] { nameof(OrderType) });
            }
            OrderType = orderType.ToString();

            if (!Enum.TryParse(OrderParameterType, true, out OrderParameterType orderParameterType))
            {
                yield return new ValidationResult("Invalid OrderParameterType", new[] { nameof(OrderParameterType) });
            }
            OrderParameterType = orderParameterType.ToString();

            if(Price == decimal.Zero){
                yield return new ValidationResult("Order must have a valid price", new[] { nameof(OrderType) });
            }

            if (StopPrice is null && (OrderType == "StopMarket" || OrderType == "TakeProfitMarket"))
            {
                yield return new ValidationResult("StopPrice is required for OrderType", new[] { nameof(OrderType) });
            }
        }
    }
}