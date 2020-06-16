// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class NORGate : Gate
    {
        protected override bool Check() { return !Inputs.Any(c => c.IsActive); }
    }
}