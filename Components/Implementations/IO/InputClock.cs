﻿using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private readonly long _ms;

        public InputClock(IPanel parent) : this(parent, 1000) { }

        public InputClock(IPanel parent, long ms) : base(parent)
        {
            //TODO Popup when Constructing
            _ms = ms;
            new Thread(() =>
                       {
                           var stopwatch = new Stopwatch();
                           stopwatch.Start();
                           while (stopwatch.ElapsedMilliseconds < 10_000)
                           {
                               Dispatcher.UIThread.InvokeAsync(() =>
                                                               {
                                                                   InputBox.IsChecked = !InputBox.IsChecked;
                                                                   CheckboxOnClick(null, null);
                                                               });
                               Thread.Sleep(1000);
                           }
                       }).Start();
        }
    }
}