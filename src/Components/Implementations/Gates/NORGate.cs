// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia;

namespace BetterHades.Components.Implementations.Gates
{
    public class NORGate : Gate
    {
        public NORGate(Point pos, bool isActive) : base(pos, isActive) { }

        protected override bool Check() { return !Inputs.Any(c => c.IsActive); }
    }
}