using System;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public class Connection : IObserver<IComponent>, IObservable<Connection>
    {
        private readonly IComponent _input;
        private ObservingComponent _output;

        public Connection(IComponent input, ObservingComponent output)
        {
            _input = input;
            _input.Subscribe(this);
            Subscribe(output);
            _output.AddInput(this);
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

        public void OnNext(IComponent input) { Notify(); }

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