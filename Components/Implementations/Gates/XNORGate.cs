// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class XNORGate : Gate
    {
        protected override bool Check() { return Inputs.All(c => c == Inputs[0].IsActive); }
    }
}