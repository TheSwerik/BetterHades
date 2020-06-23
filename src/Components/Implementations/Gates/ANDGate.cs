// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class ANDGate : Gate
    {
        public ANDGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return Inputs.All(c => c.IsActive); }
    }
}