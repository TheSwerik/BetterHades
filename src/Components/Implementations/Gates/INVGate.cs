// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia;

namespace BetterHades.Components.Implementations.Gates
{
    public class INVGate : Gate
    {
        public INVGate(Point pos, bool isActive) : base(pos, isActive, "!") { }

        protected override bool Check() { return Inputs.Count > 0 && !Inputs.First().IsActive; }

        // Inverse always has only one output.
        public override void AddInput(Connection connection)
        {
            //TODO remove connection
            Inputs.Clear();
            Inputs.Add(connection);
        }
    }
}