// ReSharper disable InconsistentNaming

using System.Linq;

namespace BetterHades.Components.Implementations.Gates
{
    public class INV : Gate
    {
        protected override bool Check() { return !Inputs.First().IsActive; }

        // Inverse always has only one output.
        public override void AddInput(Connection connection)
        {
            Inputs.Clear();
            Inputs.Add(connection);
        }
    }
}