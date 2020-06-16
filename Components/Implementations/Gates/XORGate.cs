// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class XORGate : Gate
    {
        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}