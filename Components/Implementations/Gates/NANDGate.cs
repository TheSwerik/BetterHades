// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia.Controls;

namespace BetterHades.Components.Implementations.Gates
{
    public class NANDGate : Gate
    {
        public NANDGate(IPanel parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return !Inputs.All(c => c.IsActive); }
    }
}