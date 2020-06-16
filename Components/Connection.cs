using System;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public class Connection : IObserver<IComponent>, IObservable<Connection>
    {
        private readonly IComponent _input;
        private IObservingComponent _output;

        public Connection(IComponent input, IObservingComponent output)
        {
            _input = input;
            Subscribe(output);
        }

        /**
        * Subscribes the Observer to this Connection.
        */
        public IDisposable Subscribe(IObserver<Connection> observer)
        {
            _output = (IObservingComponent) observer;
            return (IDisposable) observer;
        }

        // Observer-Stuff
        public void OnCompleted() { throw new CompletedException(); }

        public void OnError(Exception error) { Console.WriteLine("CONNECTION --- ", error); }

        public void OnNext(IComponent input) { Notify(input.IsActive()); }

        public void Notify(bool b)
        {
            _output.OnNext(this);
            Console.WriteLine("CONNECTION ONNEXT");
        }

        // Getter
        public bool IsActive() { return _input.IsActive(); }

        // Overrides
        public override int GetHashCode() { return IsActive().GetHashCode(); }

        public static bool operator ==(Connection connection, bool comparator)
        {
            return connection?.IsActive() == comparator;
        }

        public static bool operator !=(Connection connection, bool comparator)
        {
            return connection?.IsActive() != comparator;
        }

        private bool Equals(Connection other) { return IsActive() == other.IsActive(); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Connection) obj);
        }

        public override string ToString() { return IsActive() + ""; }
    }
}