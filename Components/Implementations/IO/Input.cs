using Avalonia.Controls;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        public readonly CheckBox InputBox;

        public Input(CheckBox inputBox) { InputBox = inputBox; }

        public void Update()
        {
            if (InputBox.IsChecked != null) Notify(IsActive = (bool) InputBox.IsChecked);
        }
    }
}