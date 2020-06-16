﻿using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputClock : Input
    {
        private readonly long _ms;

        public InputClock(CheckBox inputBox, long ms) : base(inputBox)
        {
            _ms = ms;
            new Thread(() =>
                       {
                           var stopwatch = new Stopwatch();
                           stopwatch.Start();
                           while (stopwatch.ElapsedMilliseconds < 10_000)
                               if (stopwatch.ElapsedMilliseconds % _ms < 10)
                                   Dispatcher.UIThread.InvokeAsync(() => InputBox.IsChecked = !InputBox.IsChecked);
                       }).Start();
        }
    }
}