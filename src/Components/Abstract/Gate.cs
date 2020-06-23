using System.Collections.Generic;
using Avalonia;

namespace BetterHades.Components
{
    public abstract class Gate : ObservingComponent
    {
        protected Gate(Point pos, bool isActive) : base(pos, isActive) { }
        protected abstract bool Check();
        protected override void Update() { Notify(IsActive = Check()); }

        protected override List<Point> GetPoints()
        {
            return new List<Point>
                   {
                       new Point(Pos.X - MainWindow.GridCellSize, Pos.Y - MainWindow.GridCellSize),
                       new Point(Pos.X + MainWindow.GridCellSize, Pos.Y - MainWindow.GridCellSize),
                       new Point(Pos.X + MainWindow.GridCellSize, Pos.Y + MainWindow.GridCellSize),
                       new Point(Pos.X - MainWindow.GridCellSize, Pos.Y + MainWindow.GridCellSize)
                   };
        }
    }
}