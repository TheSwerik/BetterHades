using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BetterHades.Components
{
    public class Connection : IObservable<Connection>, IObserver<Gate>
    {
        private readonly ObservableCollection<Gate> _inComponents;
        private readonly List<Gate> _observers;
        private bool _isActive;

        public Connection()
        {
            _observers = new List<Gate>();
            _inComponents = new ObservableCollection<Gate>();
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
        public IDisposable Subscribe(IObserver<Connection> observer)
        {
            _observers.Add(observer as Gate);
            return (observer as IDisposable)!;
        }

        // Observer
        public void OnNext(Gate gate) { IsActive = gate.Output; }
        public void OnCompleted() { Console.WriteLine("COMPLETED"); }
        public void OnError(Exception error) { Console.WriteLine(error); }

        // Output Methods
        public void Activate() { IsActive = true; }
        public void Deactivate() { IsActive = false; }

        private void Notify(bool b)
        {
            foreach (var obs in _observers) obs.OnNext(this);
        }

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