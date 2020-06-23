// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : ObservingComponent
    {
        private Connection _inConnection;

        public Output(Point pos, bool isActive) : base(pos, isActive)
        {
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
            App.MainWindow.GridCanvas.Canvas.Children.Remove(OutPointCircle);
        }

        public override Point OutPoint => new Point(-9999999, -9999999);

        protected sealed override void Update()
        {
            Polygon.Fill = (IsActive = _inConnection.IsActive) ? Brushes.Red : Brushes.Gray;
        }

        public override void AddInput(Connection connection) { _inConnection = connection; }

        protected override List<Point> GetPoints()
        {
            return new List<Point>
                   {
                       InPoint,
                       Pos.WithY(Pos.Y - MainWindow.GridCellSize),
                       new Point(Pos.X + MainWindow.GridCellSize, Pos.Y - MainWindow.GridCellSize),
                       new Point(Pos.X + MainWindow.GridCellSize, Pos.Y + MainWindow.GridCellSize),
                       Pos.WithY(Pos.Y + MainWindow.GridCellSize)
                   };
        }
    }
}