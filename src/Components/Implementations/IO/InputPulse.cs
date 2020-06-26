// ReSharper disable MemberCanBeProtected.Global

using System.Threading;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

namespace BetterHades.Components.Implementations.IO
{
    public class InputPulse : Input
    {
        // ReSharper disable once IntroduceOptionalParameters.Global
        public InputPulse(Point pos, bool isActive) : this(pos, isActive, null) { }

        public InputPulse(Point pos, bool isActive, string name) : base(pos, isActive, $"i{Counter++}") { }

        protected override void OnClick(object sender, PointerPressedEventArgs e)
        {
            if (!e.GetCurrentPoint(App.MainWindow).Properties.IsLeftButtonPressed) return;
            Notify(IsActive = !IsActive);
            Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
            new Thread(() =>
                       {
                           Thread.Sleep(100);
                           Dispatcher.UIThread.InvokeAsync(() =>
                                                           {
                                                               Notify(IsActive = !IsActive);
                                                               Polygon.Fill = IsActive ? Brushes.Red : Brushes.Gray;
                                                           });
                       }).Start();
        }
    }
}