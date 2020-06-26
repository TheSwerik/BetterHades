// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia;

namespace BetterHades.Components.Implementations.Gates
{
    public class ORGate : Gate
    {
        public ORGate(Point pos, bool isActive) : base(pos, isActive, "≥1") { }
        protected override bool Check() { return Inputs.Any(c => c.IsActive); }
    }
}