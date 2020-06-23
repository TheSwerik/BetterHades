using System.Collections.Generic;
using Avalonia;
using BetterHades.Frontend;

namespace BetterHades.Components
{
    public abstract class Gate : ObservingComponent
    {
        protected Gate(GridCanvas parent, double x, double y, bool isActive) :
            base(parent, x, y, isActive, new Point(x + 10, y + 5), new Point(x - 10, y + 5))
        {
        }

        protected abstract bool Check();
        protected override void Update() { Notify(IsActive = Check()); }

        protected override List<Point> GetPoints()
        {
            return new List<Point>
                   {
                       new Point(Pos.X - 10, Pos.Y - 10),
                       new Point(Pos.X + 10, Pos.Y - 10),
                       new Point(Pos.X + 10, Pos.Y + 10),
                       new Point(Pos.X - 10, Pos.Y + 10)
                   };
        }
    }
}