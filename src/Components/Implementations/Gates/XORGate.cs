// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia;

namespace BetterHades.Components.Implementations.Gates
{
    public class XORGate : Gate
    {
        public XORGate(Point pos, bool isActive) : base(pos, isActive, "=1") { }
        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}