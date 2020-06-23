using System.Diagnostics;
using System.Threading;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private readonly long _duration;

        // TODO remove this constructor
        // ReSharper disable once IntroduceOptionalParameters.Global
        public InputClock(double x, double y, bool isActive) : this(x, y, isActive, 1000) { }

        public InputClock(double x, double y, bool isActive, long duration)
            : base(x, y, isActive)
        {
            //TODO Popup when Constructing
            _duration = duration;
            new Thread(() =>
                       {
                           var stopwatch = new Stopwatch();
                           stopwatch.Start();
                           while (stopwatch.ElapsedMilliseconds < 10_000)
                           {
                               Dispatcher.UIThread.InvokeAsync(() => { OnClick(null, null); });
                               Thread.Sleep(1000);
                           }
                       }).Start();
        }

        public double MsToSec() { return (double) _duration / 1000; }
    }
}