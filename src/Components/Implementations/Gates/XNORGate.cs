// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class XNORGate : Gate
    {
        public XNORGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return Inputs.All(c => c == Inputs[0].IsActive); }
    }
}