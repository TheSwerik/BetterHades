using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        protected readonly CheckBox InputBox;

        public Input(IPanel parent)
        {
            InputBox = new CheckBox();
            parent.Children.Add(InputBox);
            InputBox.Click += CheckboxOnClick;
        }

        public void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            if (InputBox.IsChecked != null) Notify(IsActive = (bool) InputBox.IsChecked);
        }
    }
}