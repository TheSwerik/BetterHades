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
        public readonly Component Input;
        public ObservingComponent Output;
        private readonly Polyline _line;

        public Connection(Component input, ObservingComponent output, IPanel parent)
        {
            Input = input;
            Input.Subscribe(this);
            Subscribe(output);
            Output.AddInput(this);
            _line = new Polyline
                    {
                        Points = new List<Point> {Input.OutPoint.Bounds.Center, Output.InPoint.Bounds.Center},
                        Stroke = Brushes.Green
                    };
            parent.Children.Add(_line);
        }

        public bool IsActive => Input.IsActive;

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

        public void OnNext(Component input) { Notify(); }

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

        public override string ToString() { return $"{Input} {Output} {IsActive}"; }
    }
}