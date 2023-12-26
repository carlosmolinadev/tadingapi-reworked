using System;
using Riok.Mapperly.Abstractions;
using TradingApp.Application.Models.Requests;
using TradingApp.Domain.Entities;

//[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByValue)]
//[Mapper(PropertyNameMappingStrategy = PropertyNameMappingStrategy.CaseInsensitive)]
[Mapper]
public static partial class TradeMapper
{
    [MapperIgnoreTarget(nameof(Trade.Id))]
    public static partial Trade ToTrade(AddTradeRequest request);
    public static OrderParameter ToOrderParameter(OrderParameterRequest request, Symbol symbol)
    {
        if (request.Quantity == decimal.Zero && request.Amount != decimal.Zero)
        {
            request.Quantity = request.Amount / request.Price;
        }

        request.Quantity = symbol.FormatQuantity(request.Quantity);
        request.Price = symbol.FormatPrice(request.Price);
        request.StopPrice = request.StopPrice != null ? symbol.FormatPrice((decimal)request.StopPrice) : null;
        return ToOrderParameterMap(request);
    }

    private static partial OrderParameter ToOrderParameterMap(OrderParameterRequest request);
    private static decimal GetPower10(int digits)
    {
        return (decimal)Math.Pow(10, digits);
    }
}