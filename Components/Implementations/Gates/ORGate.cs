// ReSharper disable InconsistentNaming
using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class ORGate : Gate
    {
        protected override bool Check() { return Inputs.Any(c => c.IsActive); }
    }
}