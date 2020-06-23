// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class NANDGate : Gate
    {
        public NANDGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return !Inputs.All(c => c.IsActive); }
    }
}