using System;
using System.ComponentModel.DataAnnotations;
using TradingApp.Domain.Entities;
using TradingApp.Domain.Enums;

namespace TradingApp.Application.Models.Requests
{
    public class AddTradeRequest : IValidatableObject
    {
        [Required]
        public string SymbolName { get; set; }
        [Required]
        public required string Side { get; set; }
        public decimal RiskReward { get; set; }
        public int DelayedEntry { get; set; }
        public int RetryAttempt { get; set; }
        public decimal CloseValue { get; set; }
        public string? CloseCondition { get; set; }
        public bool CandleClosedEntry { get; set; }
        public bool UseAccountQuantity { get; set; }
        public string? OrderCloseParameterType { get; set; }
        public Guid AccountId { get; set; }
        public bool UseDefaultTradeParameter { get; set; }
        public string? CloseTradeOrder { get; set; }
        internal Symbol? Symbol { get; set; }
        public IEnumerable<OrderParameterRequest>? OrderParameters { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Enum.TryParse(Side, true, out OrderSide result))
            {
                yield return new ValidationResult("Invalid OrderSide", new[] { nameof(OrderSide) });
            }
            Side = result.ToString();

            if (!string.IsNullOrEmpty(CloseCondition))
            {
                if (!Enum.TryParse(CloseCondition, true, out CloseConditionOption conditionOption))
                {
                    yield return new ValidationResult("Invalid CloseCondition", new[] { nameof(CloseConditionOption) });
                }
                CloseCondition = conditionOption.ToString();
                if (CloseCondition != nameof(CloseConditionOption.CandleWick) && CloseValue == decimal.Zero)
                {
                    yield return new ValidationResult("Invalid CloseValue", new[] { nameof(CloseConditionOption) });
                }
            }
            if (!string.IsNullOrEmpty(OrderCloseParameterType) && Enum.TryParse(OrderCloseParameterType, true, out OrderParameterType ocpt))
            {
                if (ocpt != OrderParameterType.StopLoss || ocpt != OrderParameterType.TakeProfit)
                {
                    yield return new ValidationResult("Invalid OrderCloseParameterType", new[] { nameof(OrderParameterType) });
                }
                OrderCloseParameterType = ocpt.ToString();
            }

            if (UseAccountQuantity && !UseDefaultTradeParameter && string.IsNullOrEmpty(OrderCloseParameterType))
            {
                yield return new ValidationResult("Cannot use account quantity without default trade parameters", new[] { nameof(OrderParameterType) });
            }

            if (OrderParameters == null)
            {
                yield return new ValidationResult("At least an order is required", new[] { nameof(OrderParameterRequest) });
            }

            if (!OrderParameters!.Any(order => order.OrderParameterType == nameof(OrderParameterType.Open)))
            {
                yield return new ValidationResult("Open order is required", new[] { nameof(OrderParameterType) });
            }

            if (OrderParameters!.Count(order => order.OrderParameterType == nameof(OrderParameterType.Open)) > 1)
            {
                yield return new ValidationResult("Cannot handle more than one open trade at the same time", new[] { nameof(OrderParameterType) });
            }
            if (OrderParameters!.Where(trade => trade.OrderParameterType == "Open").Any(openTrade => openTrade.Amount == decimal.Zero && ((!UseAccountQuantity && string.IsNullOrEmpty(CloseCondition) && openTrade.Quantity == decimal.Zero) || openTrade.Price == decimal.Zero)))
            {
                yield return new ValidationResult("Invalid state for OpenOrder", new[] { nameof(OrderSide) });
            }
        }
    }
}