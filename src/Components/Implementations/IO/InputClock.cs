using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private readonly long _duration;

        // TODO remove this constructor
        // ReSharper disable once IntroduceOptionalParameters.Global
        public InputClock(Point pos, bool isActive) : this(pos, isActive, 1000) { }
        public InputClock(Point pos, bool isActive, string name) : this(pos, isActive, name, 1000) { }

        public InputClock(Point pos, bool isActive, long duration) : this(pos, isActive, null) { }

        public InputClock(Point pos, bool isActive, string name, long duration) : base(pos, isActive, name)
        {
            //TODO Popup when Constructing
            _duration = duration;
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

        public double MsToSec() { return (double) _duration / 1000; }
    }
}