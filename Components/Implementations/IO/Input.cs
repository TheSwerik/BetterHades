using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public CheckBox InputBox { get; }

        public Input(IPanel parent)
        {
            InputBox = new CheckBox();
            parent.Children.Add(InputBox);
            InputBox.Click += CheckboxOnClick;
        }

        protected void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            if (InputBox.IsChecked != null) Notify(IsActive = (bool) InputBox.IsChecked);
        }
    }
}