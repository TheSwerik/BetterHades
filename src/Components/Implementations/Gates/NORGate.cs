// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class NORGate : Gate
    {
        public NORGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return !Inputs.Any(c => c.IsActive); }
    }
}