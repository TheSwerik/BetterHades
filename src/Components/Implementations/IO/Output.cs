// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : ObservingComponent
    {
        public static int Counter = 1;
        private string _name = $"o{Counter}";

        public Output(Point pos, bool isActive) : this(pos, isActive, null) { }

        public Output(Point pos, bool isActive, string name) : base(pos, isActive, $"o{Counter++}")
        {
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
            App.MainWindow.GridCanvas.Canvas.Children.Remove(OutPointCircle);
            Canvas.SetLeft(Text, Pos.X - MainWindow.GridCellSize * 0.8);
            if (name == null || Name.Equals(name)) return;
            Name = name;
            Counter--;
        }

        protected override double PositionMultiplier => 1 - 0.2 * (Text.FontSize / MainWindow.GridCellSize * 1.4);

        public string Name
        {
            get => _name;
            set
            {
                Text.Text = _name = value;
                Text.FontSize = (double) MainWindow.GridCellSize / _name.Length * 2;
                Canvas.SetTop(Text, Pos.Y - MainWindow.GridCellSize * 0.75 * (Text.FontSize / MainWindow.GridCellSize));
                Canvas.SetLeft(Text, Pos.X - MainWindow.GridCellSize * PositionMultiplier);
            }
        }

        public override Point OutPoint => new Point(double.MinValue, double.MinValue);

        public override void MoveTo(Point pos)
        {
            base.MoveTo(pos);
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
            App.MainWindow.GridCanvas.Canvas.Children.Remove(OutPointCircle);
        }

        protected sealed override void Update()
        {
            Polygon.Fill = (IsActive = Inputs.Count > 0 && Inputs.All(c => c.IsActive)) ? Brushes.Red : Brushes.Gray;
        }

        public override void AddInput(Connection connection)
        {
            //TODO remove connection
            Inputs.Clear();
            Inputs.Add(connection);
        }

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