using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private readonly long _duration;

        // TODO remove this constructor
        // ReSharper disable once IntroduceOptionalParameters.Global
        public InputClock(Point pos, bool isActive) : this(pos, isActive, 1000) { }

        public InputClock(Point pos, bool isActive, long duration) : base(pos, isActive)
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