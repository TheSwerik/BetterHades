// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia.Controls;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.Gates
{
    public class ORGate : Gate
    {
        public ORGate(GridCanvas parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return Inputs.Any(c => c.IsActive); }
    }
}