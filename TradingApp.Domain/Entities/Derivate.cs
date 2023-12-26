using TradingApp.Domain.Common;

namespace TradingApp.Domain.Entities
{
    public class Derivate : Entity<int>
    {
        private Derivate(){}
        public Derivate(int id,string name, int exchangeId, int symbolId) : base(id)
        {
            Name = name;
            ExchangeId = exchangeId;
            SymbolId = symbolId;
        }
        public string Name { get; set; }
        public int ExchangeId { get; set; }
        public int SymbolId { get; set; }
    }
}