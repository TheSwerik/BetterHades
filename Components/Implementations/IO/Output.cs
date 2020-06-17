// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : Component, IObservingComponent
    {
        public Output(IPanel parent, double x, double y) : base(parent, x, y) { }

        public void Update(Connection connection) { ChangeColor(connection.IsActive); }
        public void AddInput(Connection connection) { ; }

        private void ChangeColor(bool active) { _polygon.Fill = active ? Brushes.Red : Brushes.Gray; }

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