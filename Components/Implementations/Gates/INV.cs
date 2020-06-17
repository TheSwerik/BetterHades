// ReSharper disable InconsistentNaming

using System.Linq;
using Avalonia.Controls;

namespace BetterHades.Components.Implementations.Gates
{
    public class INV : Gate
    {
        public INV(IPanel parent, double x, double y) : base(parent, x, y) { }

        protected override bool Check() { return !Inputs.First().IsActive; }

        // Inverse always has only one output.
        public override void AddInput(Connection connection)
        {
            Inputs.Clear();
            Inputs.Add(connection);
        }
    }
}