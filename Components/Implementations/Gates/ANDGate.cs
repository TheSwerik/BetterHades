// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using Avalonia.Controls;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.Gates
{
    public class ANDGate : Gate
    {
        public ANDGate(GridCanvas parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check()
        {
            Console.WriteLine("---------- AND ----------");
            for (var i = 0; i < Inputs.Count; i++)
            {
                Console.WriteLine("{0}: {1}", i, Inputs[i]);
            }

            return Inputs.All(c => c.IsActive);
        }
    }
}