// ReSharper disable InconsistentNaming

using System.Collections.ObjectModel;
using System.Linq;
using BetterHades.Components.Implementations.IO;

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