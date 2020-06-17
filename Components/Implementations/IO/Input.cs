// ReSharper disable MemberCanBeProtected.Global

using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public Input(IPanel parent, double x, double y) : base(parent, x, y)
        {
            _polygon.PointerPressed += CheckboxOnClick;
        }

        protected CheckBox InputBox { get; }

        protected void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            if (InputBox.IsChecked != null) Notify(IsActive = (bool) InputBox.IsChecked);
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