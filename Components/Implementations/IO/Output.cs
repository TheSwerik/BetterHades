// ReSharper disable ClassNeverInstantiated.Global

using Avalonia.Controls;
using Avalonia.Media;

namespace BetterHades.Components.Implementations.IO
{
    public class Output : Component, IObservingComponent
    {
        private readonly TextBlock _rectangle;

        public Output(IPanel parent, double x, double y)
        {
            _rectangle = new TextBlock {Background = Brushes.Gray, Width = 100, Height = 100};
            parent.Children.Add(_rectangle);
            Canvas.SetLeft(_rectangle, x);
            Canvas.SetTop(_rectangle, y);
        }

        public void Update(Connection connection) { ChangeColor(connection.IsActive); }
        public void AddInput(Connection connection) { ; }

        private void ChangeColor(bool active) { _rectangle.Background = active ? Brushes.Red : Brushes.Gray; }
    }
}