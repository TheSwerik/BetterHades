// ReSharper disable MemberCanBeProtected.Global

using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public Input(IPanel parent, double x, double y)
        {
            InputBox = new CheckBox();
            parent.Children.Add(InputBox);
            InputBox.Click += CheckboxOnClick;
            Canvas.SetLeft(InputBox, x);
            Canvas.SetTop(InputBox, y);
        }

        protected CheckBox InputBox { get; }

        protected void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            if (InputBox.IsChecked != null) Notify(IsActive = (bool) InputBox.IsChecked);
        }
    }
}