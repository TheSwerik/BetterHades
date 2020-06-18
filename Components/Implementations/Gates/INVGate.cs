// ReSharper disable InconsistentNaming

using System.Linq;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.Gates
{
    public class INVGate : Gate
    {
        public INVGate(GridCanvas parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return Inputs.Count > 0 && !Inputs.First().IsActive; }

        // Inverse always has only one output.
        public override void AddInput(Connection connection)
        {
            Inputs.Clear();
            Inputs.Add(connection);
        }
    }
}