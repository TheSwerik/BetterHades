using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Component
    {
        public readonly CheckBox InputBox;
        private readonly long _ms;

        public InputClock(CheckBox inputBox, long ms)
        {
            InputBox = inputBox;
            _ms = ms;
            new Thread(() =>
                       {
                           var stopwatch = new Stopwatch();
                           while (stopwatch.ElapsedMilliseconds < 10_000)
                           {
                               if (stopwatch.ElapsedMilliseconds % _ms < 10) InputBox.IsChecked = !InputBox.IsChecked;
                           }
                       }).Start();
        }

        public void Update()
        {
            if (InputBox.IsChecked != null) Notify(IsActive = (bool) InputBox.IsChecked);
        }
    }
}