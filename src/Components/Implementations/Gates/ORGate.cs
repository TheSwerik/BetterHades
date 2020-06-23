// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class ORGate : Gate
    {
        public ORGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return Inputs.Any(c => c.IsActive); }
    }
}