// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class XORGate : Gate
    {
        public XORGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}