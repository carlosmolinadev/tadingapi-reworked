using TradingApp.Domain.Common;

namespace TradingApp.Domain.Entities
{
    public sealed class TradeOrder : Entity<int>
    {
        public TradeOrder(decimal quantity, decimal? stopPrice, decimal limitPrice, OrderSide sideId, OrderType typeId, OrderStatus statusId, Guid tradeId, OrderParameterType orderParameterType, Guid? referenceNumber) : base(default)
        {
            Quantity = quantity;
            StopPrice = stopPrice;
            LimitPrice = limitPrice;
            SideId = sideId;
            TypeId = typeId;
            StatusId = statusId;
            TradeId = tradeId;
            if (orderParameterType != OrderParameterType.Open && referenceNumber is null)
            {
                throw new ArgumentException("Cannot open conditionals without open order");
            }
            OrderParameterType = orderParameterType;
            ReferenceNumber = Guid.NewGuid();
        }

        public void ChangeOrderStatus(OrderStatus status)
        {
            StatusId = status;
        }
        public void SetQuantity(decimal quantity)
        {
            Quantity = quantity;
        }
        public void SetLimitPrice(decimal price)
        {
            LimitPrice = price;
        }
        #region Properties
        public decimal Quantity { get; private set; }
        public decimal? StopPrice { get; private set; }
        public decimal LimitPrice { get; private set; }
        public OrderParameterType OrderParameterType { get; init; }
        public OrderSide SideId { get; init; }
        public OrderType TypeId { get; init; }
        public OrderStatus StatusId { get; private set; }
        public int? OpenTradeOrder { get; private set; }
        public Guid TradeId { get; init; }
        public Guid ReferenceNumber { get; init; }

        #endregion
    }
}