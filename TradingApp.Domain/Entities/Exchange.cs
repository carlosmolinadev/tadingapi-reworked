using TradingApp.Domain.Common;

namespace TradingApp.Domain.Entities
{
    public class Exchange : Entity<int>
    {
        private Exchange() : base (0){}
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        private readonly List<Derivate> _derivates = new();
        public IReadOnlyCollection<Derivate> Derivates => _derivates;
    }
}