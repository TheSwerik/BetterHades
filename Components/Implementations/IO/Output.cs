// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : ObservingComponent
    {
        private Connection _inConnection;

        public Output(GridCanvas parent, double x, double y)
            : base(parent, x, y, new Point(-999999, -999999), new Point(x, y))
        {
        }

        protected override void Update() { ChangeColor(_inConnection.IsActive); }
        public override void AddInput(Connection connection) { _inConnection = connection; }
        private void ChangeColor(bool active) { Polygon.Fill = active ? Brushes.Red : Brushes.Gray; }

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