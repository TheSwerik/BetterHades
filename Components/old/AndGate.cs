using System.Linq;

namespace BetterHades.Components.Gates
{
    public class AndGate : Gate
    {
        protected override bool Check() { return InConnections.All(c => c == true); }
    }
}