// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : ObservingComponent
    {
        private Connection _inConnection;

        public Output(double x, double y, bool isActive)
            : base(x, y, isActive, new Point(-999999, -999999), new Point(x, y))
        {
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected sealed override void Update()
        {
            Polygon.Fill = (IsActive = _inConnection.IsActive) ? Brushes.Red : Brushes.Gray;
        }

        public override void AddInput(Connection connection) { _inConnection = connection; }

        protected override List<Point> GetPoints()
        {
            return new List<Point>
                   {
                       Pos,
                       new Point(Pos.X + 10, Pos.Y - 10),
                       new Point(Pos.X + 20, Pos.Y - 10),
                       new Point(Pos.X + 20, Pos.Y + 10),
                       new Point(Pos.X + 10, Pos.Y + 10)
                   };
        }
    }
}