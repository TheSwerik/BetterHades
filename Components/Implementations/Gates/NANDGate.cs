// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class NANDGate : Gate
    {
        protected override bool Check() { return !Inputs.All(c => c.IsActive); }
    }
}