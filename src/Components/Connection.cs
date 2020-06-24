using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public class Connection : IObserver<Component>, IObservable<Connection>
    {
        public readonly Component Input;
        private Polyline _line;
        public ObservingComponent Output;

        public Connection(Component input, ObservingComponent output, Polyline line)
        {
            Input = input;
            Input.Subscribe(this);
            Subscribe(output);
            Output.AddInput(this);
            _line = new Polyline {Points = line.Points, Stroke = IsActive ? Brushes.Red : Brushes.Gray, ZIndex = -9999};
            App.MainWindow.GridCanvas.Canvas.Children.Add(_line);
            Notify();
        }

        public bool IsActive => Input.IsActive;
        public IEnumerable<Point> Points => _line.Points;

        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<Connection> observer)
        {
            Output = (ObservingComponent) observer;
            return (observer as IDisposable)!;
        }

        // Observer-Stuff
        public void OnCompleted() { throw new CompletedException(); }
        public void OnError(Exception error) { Console.WriteLine("CONNECTION --- {0}", error); }

        public void OnNext(Component input)
        {
            Notify();
            _line.Stroke = IsActive ? Brushes.Red : Brushes.Gray;
        }

        private void Notify() { Output.OnNext(this); }

        // Overrides
        public override int GetHashCode() { return IsActive.GetHashCode(); }

        public static bool operator ==(Connection connection, bool comparator)
        {
            return connection?.IsActive == comparator;
        }

        public static bool operator !=(Connection connection, bool comparator)
        {
            return connection?.IsActive != comparator;
        }

        private bool Equals(Connection other) { return IsActive == other.IsActive; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Connection) obj);
        }

        public override string ToString() { return $"{Input} {Output} {IsActive} {string.Join(",", _line.Points)}"; }

        public void UpdateLine(bool input)
        {
            App.MainWindow.GridCanvas.Canvas.Children.Remove(_line);
            if (input) _line.Points[0] = Input.OutPoint;
            else _line.Points[^1] = Output.InPoint;
            _line = new Polyline
                    {Points = _line.Points, Stroke = IsActive ? Brushes.Red : Brushes.Gray, ZIndex = -9999};
            App.MainWindow.GridCanvas.Canvas.Children.Add(_line);
        }
    }
}