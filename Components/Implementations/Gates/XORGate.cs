// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia.Controls;

namespace BetterHades.Components.Implementations.Gates
{
    public class XORGate : Gate
    {
        public XORGate(IPanel parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}