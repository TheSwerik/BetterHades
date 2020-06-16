using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class AndGate : Gate
    {
        protected override bool Check() { return Inputs.All(c => c.IsActive); }
    }
}