// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia;

namespace BetterHades.Components.Implementations.Gates
{
    public class XNORGate : Gate
    {
        public XNORGate(Point pos, bool isActive) : base(pos, isActive) { }

        protected override bool Check() { return Inputs.All(c => c == Inputs[0].IsActive); }
    }
}