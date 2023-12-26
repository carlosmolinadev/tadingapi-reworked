using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingApp.Domain.Entities
{
    public record ExchangeKlineData
    {
        public ExchangeKlineData(decimal openPrice, decimal highPrice, decimal lowPrice, decimal closePrice, bool final, string symbol)
        {
            OpenPrice = openPrice;
            HighPrice = highPrice;
            LowPrice = lowPrice;
            ClosePrice = closePrice;
            Final = final;
            Symbol = symbol;
        }

        // Summary:
        //     The price at which this candlestick opened
        public decimal OpenPrice { get; set; }
        //
        // Summary:
        //     The highest price in this candlestick
        public decimal HighPrice { get; set; }
        //
        // Summary:
        //     The lowest price in this candlestick
        public decimal LowPrice { get; set; }
        //
        // Summary:
        //     The price at which this candlestick closed
        public decimal ClosePrice { get; set; }
        //
        // Summary:
        //     Is this kline final
        public bool Final { get; set; }
        //
        // Summary:
        //     The Kline Symbol
        public string Symbol { get; set; }
    }
}