// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        private static int _counter = 1;
        private string _name = $"i{_counter}";

        public Input(Point pos, bool isActive) : base(pos, isActive, $"i{_counter++}")
        {
            Polygon.PointerPressed += OnClick;
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected override double PositionMultiplier => 1 + 0.2 * (Text.FontSize / MainWindow.GridCellSize * 1.4);

        public string Name
        {
            get => _name;
            set
            {
                Text.Text = _name = value;
                Text.FontSize = (double) MainWindow.GridCellSize * _name.Length / 27;
                Canvas.SetTop(Text, Pos.Y - MainWindow.GridCellSize * 0.75 * (Text.FontSize / MainWindow.GridCellSize));
                Canvas.SetLeft(Text, Pos.X - MainWindow.GridCellSize * PositionMultiplier);
            }
        }

        public override void MoveTo(Point pos)
        {
            base.MoveTo(pos);
            Polygon.PointerPressed += OnClick;
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected void OnClick(object sender, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(App.MainWindow).Properties.IsLeftButtonPressed) return;
            Notify(IsActive = !IsActive);
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected override List<Point> GetPoints()
        {
            return new List<Point>
                   {
                       OutPoint,
                       Pos.WithY(Pos.Y - MainWindow.GridCellSize),
                       new Point(Pos.X - MainWindow.GridCellSize, Pos.Y - MainWindow.GridCellSize),
                       new Point(Pos.X - MainWindow.GridCellSize, Pos.Y + MainWindow.GridCellSize),
                       Pos.WithY(Pos.Y + MainWindow.GridCellSize)
                   };
        }
    }
}