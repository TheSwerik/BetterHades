// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Media;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public Input(GridCanvas parent, double x, double y, bool isActive) : base(
            parent, x, y, isActive, new Point(x, y))
        {
            Polygon.PointerPressed += OnClick;
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected void OnClick(object sender, RoutedEventArgs e)
        {
            Notify(IsActive = !IsActive);
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
        }

        protected override List<Point> GetPoints(double x, double y)
        {
            return new List<Point>
                   {
                       new Point(x, y),
                       new Point(x - 10, y - 10),
                       new Point(x - 20, y - 10),
                       new Point(x - 20, y + 10),
                       new Point(x - 10, y + 10)
                   };
        }
    }
}