using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private const long Duration = 1000;

        // ReSharper disable once IntroduceOptionalParameters.Global
        public InputClock(Point pos, bool isActive) : this(pos, isActive, null) { }

        public InputClock(Point pos, bool isActive, string name) : base(pos, isActive, name)
        {
            new Thread(() =>
                       {
                           var stopwatch = new Stopwatch();
                           stopwatch.Start();
                           while (stopwatch.ElapsedMilliseconds < 10_000)
                           {
                               Dispatcher.UIThread.InvokeAsync(() =>
                                                               {
                                                                   Notify(IsActive = !IsActive);
                                                                   Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
                                                               });
                               Thread.Sleep(1000);
                           }
                       }).Start();
        }

        public double MsToSec() { return (double) Duration / 1000; }
    }
}