using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace BetterHades.Components
{
    public abstract class Gate : ObservingComponent
    {
        protected Gate(IPanel parent, double x, double y) : base(parent, x, y) { }
        protected abstract bool Check();
        protected override void Update() { Notify(IsActive = Check()); }

        protected override List<Point> GetPoints(double x, double y)
        {
            return new List<Point>
                   {
                       new Point(x - 10, y - 10),
                       new Point(x + 10, y - 10),
                       new Point(x + 10, y + 10),
                       new Point(x - 10, y + 10)
                   };
        }
    }
}