using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public class Connection : IObserver<Component>, IObservable<Connection>
    {
        private readonly Component _input;
        private ObservingComponent _output;
        private Polyline _line;

        public Connection(Component input, ObservingComponent output, IPanel parent)
        {
            _input = input;
            _input.Subscribe(this);
            Subscribe(output);
            _output.AddInput(this);
            _line = new Polyline
                    {
                        Points = new List<Point>() {_input.OutPoint.Bounds.Center, _output.InPoint.Bounds.Center},
                        Stroke = Brushes.Green
                    };
            parent.Children.Add(_line);
        }

        public bool IsActive => _input.IsActive;

        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<Connection> observer)
        {
            _output = (ObservingComponent) observer;
            return (observer as IDisposable)!;
        }

        // Observer-Stuff
        public void OnCompleted() { throw new CompletedException(); }

        public void OnError(Exception error) { Console.WriteLine("CONNECTION --- {0}", error); }

        public void OnNext(Component input) { Notify(); }

        private void Notify() { _output.OnNext(this); }

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

        public override string ToString() { return IsActive + ""; }
    }
}