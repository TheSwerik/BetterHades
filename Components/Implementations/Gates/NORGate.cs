// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia.Controls;

namespace BetterHades.Components.Implementations.Gates
{
    public class NORGate : Gate
    {
        public NORGate(IPanel parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return !Inputs.Any(c => c.IsActive); }
    }
}