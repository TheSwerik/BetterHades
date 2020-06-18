// ReSharper disable InconsistentNaming

using System.Linq;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.Gates
{
    public class XORGate : Gate
    {
        public XORGate(GridCanvas parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}