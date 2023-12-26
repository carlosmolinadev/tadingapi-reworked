using TradingApp.Domain.Common;
using TradingApp.Domain.Enums;

namespace TradingApp.Domain.Entities
{
    public class Trade : Entity<Guid>
    {
       // public Trade(decimal riskReward, int retryAttempt, bool candleClosedEntry, Symbol symbol, string side, Guid accountId, int delayedEntry, string? closeCondition, decimal closeConditionValue) : base(Guid.NewGuid())
       // {
       //     if (Enum.TryParse(side, out OrderSide orderSide))
       //     {
       //         Side = orderSide;
       //     }
       //     if (!string.IsNullOrEmpty(closeCondition))
       //     {
       //         CloseCondition = Enum.GetName(Enum.Parse<CloseConditionOption>(closeCondition));
       //         CloseConditionValue = closeConditionValue;
       //         _defaultTradeParameter = true;
       //     }
       //     RetryAttempt = retryAttempt;
       //     CandleClosedEntry = candleClosedEntry;
       //     Symbol = symbol;
       //     AccountId = accountId;
       //     Status = nameof(TradeStatus.Pending);
       //     _closeSide = Side == OrderSide.Buy ? OrderSide.Sell : OrderSide.Buy;
       //     DelayedEntry = delayedEntry;
       //     _riskRewardPercentage = decimal.Round(riskReward / 100, 2);
       // }

        public Trade(decimal riskReward, int retryAttempt, bool candleClosedEntry) : base(Guid.NewGuid())
        {
            CandleClosedEntry = candleClosedEntry;
            RiskReward = riskReward;
            RetryAttempt = retryAttempt;
        }

        #region Properties        
        public int RetryAttempt { get; private set; }
        public string Status { get; private set; }
        public bool CandleClosedEntry { get; private set; }
        public int DelayedEntry { get; private set; }
        public Symbol Symbol { get; init; }
        public OrderSide Side { get; init; }
        public Guid AccountId { get; init; }
        public string CloseCondition { get; private set; }
        public decimal CloseConditionValue { get; private set; }
        public string CloseTradeOrder { get; private set; }
        public decimal QuantityRisked { get; private set; }
        public IReadOnlyCollection<TradeOrder> TakeProfitOrders => _takeProfitOrders;
        public IReadOnlyCollection<TradeOrder> StopLossOrders => _stopLossOrders;
        public TradeOrder? TradeOrder => _openTradeOrder;

        public decimal RiskReward { get; }
        #endregion

        private bool _defaultTradeParameter;
        private TradeOrder? _openTradeOrder;
        private readonly List<TradeOrder> _takeProfitOrders = [];
        private readonly List<TradeOrder> _stopLossOrders = [];
        private readonly OrderSide _closeSide;
        private decimal _riskRewardPercentage;
        private OrderParameterType _defaultAmountConditionalType;
        private int _currentRetryEntry;
        private decimal _defaultAmount;
        private readonly decimal _riskReward;
        private readonly int _retryAttempt;

        public void SetTrade(List<OrderParameter> orders)
        {
            var openTradeOrder = orders.First(o => o.OrderParameterType == OrderParameterType.Open);
            var stopLossOrders = orders.FindAll(o => o.OrderParameterType == OrderParameterType.StopLoss);
            var takeProfitOrders = orders.FindAll(o => o.OrderParameterType == OrderParameterType.TakeProfit);

            if (string.IsNullOrEmpty(CloseCondition) && _defaultTradeParameter)
            {
                openTradeOrder.Quantity = QuantityRisked;
            }

            OpenTradeOrder(openTradeOrder);

            SetTradeConditionals(stopLossOrders, takeProfitOrders);
        }
        private void OpenTradeOrder(OrderParameter order)
        {
            if (order.Quantity == 0 && !_defaultTradeParameter)
            {
                throw new ArgumentException("Quantity cannot be determined for open Order");
            }
            _openTradeOrder = new TradeOrder(order.Quantity, order.StopPrice, order.Price, Side, order.OrderType, OrderStatus.New, Id, order.OrderParameterType, null);
        }

        public void SetTradeConditionals(List<OrderParameter> stopLossOrders, List<OrderParameter> takeProfitOrders)
        {
            if (_openTradeOrder is null)
            {
                throw new Exception("Trade order has not been set");
            }
            if (QuantityRisked < stopLossOrders.Sum(q => q.Quantity))
            {
                throw new Exception("StopLoss orders quantity cannot be higher than openOrder quantity");
            }
            if (QuantityRisked < takeProfitOrders.Sum(o => o.Quantity))
            {
                throw new Exception("TakeProfit orders quantity cannot be higher than openOrder quantity");
            }
            if (stopLossOrders.Any())
            {
                _stopLossOrders.AddRange(stopLossOrders.Select(o => new TradeOrder(o.Quantity, o.StopPrice, o.Price, _closeSide, OrderType.StopMarket, OrderStatus.New, Id, OrderParameterType.StopLoss, _openTradeOrder.ReferenceNumber)));

                if (_riskRewardPercentage != decimal.Zero && !takeProfitOrders.Any())
                {
                    var takeProfitPercentage = GetPercentageDiference(_openTradeOrder.LimitPrice, stopLossOrders[0].Price) * _riskRewardPercentage;

                    var takeProfitPrice = Symbol.FormatPrice(CalculateDefaultConditionalPriceBasedOnSide(_openTradeOrder.LimitPrice, takeProfitPercentage, OrderParameterType.TakeProfit, Side));

                    _takeProfitOrders.Add(new TradeOrder(QuantityRisked, null, takeProfitPrice, _closeSide, OrderType.Limit, OrderStatus.New, Id, OrderParameterType.TakeProfit, _openTradeOrder.ReferenceNumber));
                }
            }
            if (takeProfitOrders.Any())
            {
                _takeProfitOrders.AddRange(takeProfitOrders.Select(o => new TradeOrder(o.Quantity, o.StopPrice, o.Price, _closeSide, o.OrderType, OrderStatus.New, Id, OrderParameterType.TakeProfit, _openTradeOrder.ReferenceNumber)));
                if (_riskRewardPercentage != 0 && !_stopLossOrders.Any())
                {
                    var stopLossPercentage = GetPercentageDiference(_openTradeOrder.LimitPrice, takeProfitOrders[0].Price) * _riskRewardPercentage;
                    var stopLossPrice = Symbol.FormatPrice(CalculateDefaultConditionalPriceBasedOnSide(_openTradeOrder.LimitPrice, stopLossPercentage, OrderParameterType.StopLoss, Side));

                    _stopLossOrders.Add(new TradeOrder(QuantityRisked, null, stopLossPrice, _closeSide, OrderType.StopMarket, OrderStatus.New, Id, OrderParameterType.StopLoss, _openTradeOrder.ReferenceNumber));
                }
            }

            ValidateConditionalQuantity(_stopLossOrders);
            ValidateConditionalQuantity(_takeProfitOrders);
        }

        public bool IsTradeReady(ExchangeKlineData data)
        {
            bool ready = false;
            var triggerPrice = _openTradeOrder?.StopPrice == null ? _openTradeOrder!.LimitPrice : _openTradeOrder!.StopPrice;

            if (CandleClosedEntry == data.Final && DelayedEntry != _currentRetryEntry)
            {
                if (Side == OrderSide.Buy && triggerPrice <= data.ClosePrice)
                {
                    ready = true;
                    Status = nameof(TradeStatus.WaitingCandle);
                }
                else if (Side == OrderSide.Sell && triggerPrice >= data.ClosePrice)
                {
                    ready = true;
                }
            }
            else
            {
                if (Side == OrderSide.Buy && triggerPrice >= data.ClosePrice)
                {
                    ready = true;
                }
                else if (Side == OrderSide.Sell && triggerPrice <= data.ClosePrice)
                {
                    ready = true;
                }
            }
            return ready;
        }

        private void ValidateConditionalQuantity(List<TradeOrder> orders)
        {
            if (!orders.Any())
            {
                return;
            }

            var ordersQuantity = orders.Sum(o => o.Quantity);
            if (ordersQuantity == QuantityRisked)
            {
                return;
            }

            var openQuantityLeft = QuantityRisked - ordersQuantity;

            if (orders.Any(o => o.Quantity == decimal.Zero))
            {
                var quantityPerOrder = Symbol.FormatQuantity(openQuantityLeft / orders.Count(o => o.Quantity == decimal.Zero));
                for (var i = 0; i < orders.Count; i++)
                {
                    if (openQuantityLeft == decimal.Zero)
                    {
                        return;
                    }
                    if (i == orders.Count - 1)
                    {
                        orders[i].SetQuantity(openQuantityLeft);
                    }
                    if (orders[i].Quantity == decimal.Zero && openQuantityLeft > decimal.Zero)
                    {
                        orders[i].SetQuantity(quantityPerOrder);
                        openQuantityLeft -= quantityPerOrder;
                    }
                }
            }
            else
            {
                var updatedQuantity = orders[0].Quantity;
                orders[0].SetQuantity(updatedQuantity + openQuantityLeft);
            }
        }

        private decimal GetPercentageDiference(decimal basePrice, decimal comparativePrice)
        {
            return Math.Abs((comparativePrice - basePrice) / basePrice * 100);
        }

        private decimal CalculateDefaultConditionalPriceBasedOnSide(decimal openPrice, decimal addedPercentage, OrderParameterType conditionalType, OrderSide side)
        {
            if (conditionalType == OrderParameterType.StopLoss)
            {
                return side == OrderSide.Buy ? openPrice - (openPrice * addedPercentage) : openPrice + (openPrice * addedPercentage);
            }
            else
            {
                return side == OrderSide.Buy ? openPrice + (openPrice * addedPercentage) : openPrice - (openPrice * addedPercentage);
            }
        }

        public void LoadValuesFromDefaultTradeParameter(DefaultTradeParameter parameter)
        {
            RetryAttempt = parameter.RetryAttempt;
            CandleClosedEntry = parameter.CandleClosedEntry;
            DelayedEntry = parameter.DelayedEntry;
            CloseConditionValue = parameter.CloseValue;
            CloseCondition = parameter.CloseCondition;
            _riskRewardPercentage = decimal.Round(parameter.RiskReward / 100, 2);
            _defaultTradeParameter = true;
        }

        public void SetTradeRiskedQuantity(decimal quantity)
        {
            QuantityRisked = Symbol.FormatQuantity(quantity);
        }
    }
}