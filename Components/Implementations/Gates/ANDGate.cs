// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia.Controls;

namespace BetterHades.Components.Implementations.Gates
{
    public class ANDGate : Gate
    {
        public ANDGate(IPanel parent, double x, double y) : base(parent, x, y) { }
        protected override bool Check() { return Inputs.All(c => c.IsActive); }
    }
}