// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public Input(double x, double y, bool isActive)
            : base(x, y, isActive, new Point(x, y))
        {
            Polygon.PointerPressed += OnClick;
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected void OnClick(object sender, RoutedEventArgs e)
        {
            Notify(IsActive = !IsActive);
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected override List<Point> GetPoints()
        {
            return new List<Point>
                   {
                       Pos,
                       new Point(Pos.X - 10, Pos.Y - 10),
                       new Point(Pos.X - 20, Pos.Y - 10),
                       new Point(Pos.X - 20, Pos.Y + 10),
                       new Point(Pos.X - 10, Pos.Y + 10)
                   };
        }
    }
}