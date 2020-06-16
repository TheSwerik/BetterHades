using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class OrGate : Gate
    {
        protected override bool Check() { return Inputs.Any(c => c.IsActive); }
    }
}