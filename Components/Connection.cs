using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public class Connection : IObservable<Connection>, IObserver<Input>
    {
        private readonly Input _input;
        private readonly Gate _output;
        private bool _isActive;

        public Connection(Input input, Gate output)
        {
            _input = input;
            _input.Subscribe(this);
            _output = output;
            _isActive = _input.IsActive;
        }

        private bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value) Notify(_isActive = value);
                else _isActive = value;
            }
        }

        // Observable
        public IDisposable Subscribe(IObserver<Connection> observer) { throw new UnsubscribableException(); }

        // Observer
        public void OnNext(Input input) { IsActive = input.IsActive; }
        public void OnCompleted() { Console.WriteLine("COMPLETED"); }
        public void OnError(Exception error) { Console.WriteLine(error); }

        // Output Methods
        private void Notify(bool b) { _output.OnNext(this); }

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