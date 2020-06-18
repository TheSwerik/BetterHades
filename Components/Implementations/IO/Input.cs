// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using System.Drawing;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Brush = Avalonia.Media.Brush;
using Brushes = Avalonia.Media.Brushes;
using Point = Avalonia.Point;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public Input(IPanel parent, double x, double y) : base(parent, x, y, new Point(x, y))
        {
            Polygon.PointerPressed += OnClick;
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