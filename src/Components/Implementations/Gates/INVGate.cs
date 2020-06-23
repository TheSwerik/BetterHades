// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class INVGate : Gate
    {
        public INVGate(double x, double y, bool isActive) : base(x, y, isActive) { }

        protected override bool Check() { return Inputs.Count > 0 && !Inputs.First().IsActive; }

        // Inverse always has only one output.
        public override void AddInput(Connection connection)
        {
            Inputs.Clear();
            Inputs.Add(connection);
        }
    }
}