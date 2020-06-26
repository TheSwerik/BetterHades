// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia;

namespace BetterHades.Components.Implementations.Gates
{
    public class NANDGate : Gate
    {
        public NANDGate(Point pos, bool isActive) : base(pos, isActive, "!&") { }
        protected override bool Check() { return !Inputs.All(c => c.IsActive); }
    }
}