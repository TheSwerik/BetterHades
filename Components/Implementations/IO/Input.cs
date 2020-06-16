using Avalonia.Controls;

namespace BetterHades.Components.Implementations.IO
{
    public class Input : Component
    {
        private readonly CheckBox _input;

        public Input(CheckBox input) { _input = input; }

        public void Update()
        {
            if (_input.IsChecked != null) IsActive = (bool) _input.IsChecked;
        }
    }
}