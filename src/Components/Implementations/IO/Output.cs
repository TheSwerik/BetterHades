// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : ObservingComponent
    {
        private Connection _inConnection;

        public Output(GridCanvas parent, double x, double y, bool isActive)
            : base(parent, x, y, isActive, new Point(-999999, -999999), new Point(x, y))
        {
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected sealed override void Update()
        {
            Polygon.Fill = (IsActive = _inConnection.IsActive) ? Brushes.Red : Brushes.Gray;
        }

        public override void AddInput(Connection connection) { _inConnection = connection; }

        protected override List<Point> GetPoints(double x, double y)
        {
            return new List<Point>
                   {
                       new Point(x, y),
                       new Point(x + 10, y - 10),
                       new Point(x + 20, y - 10),
                       new Point(x + 20, y + 10),
                       new Point(x + 10, y + 10)
                   };
        }
    }
}