using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class XOrGate : Gate
    {
        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}