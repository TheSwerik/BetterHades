using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using BetterHades.Frontend;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private readonly GridCanvas _parent;
        private readonly long _ms;

        // TODO remove this constructor
        // ReSharper disable once IntroduceOptionalParameters.Global
        public InputClock(GridCanvas parent, double x, double y) : this(parent, x, y, 1000) { _parent = parent; }

        public InputClock(GridCanvas parent, double x, double y, long ms) : base(parent, x, y)
        {
            //TODO Popup when Constructing
            _ms = ms;
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
    }
}