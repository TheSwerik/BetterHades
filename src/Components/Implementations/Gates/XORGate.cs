﻿// ReSharper disable InconsistentNaming

using System.Linq;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.Gates
{
    public class XORGate : Gate
    {
        public XORGate(GridCanvas parent, double x, double y, bool isActive) : base(parent, x, y, isActive) { }

        protected override bool Check() { return Inputs.Count(c => c.IsActive) == 1; }
    }
}