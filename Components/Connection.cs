using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BetterHades.Components.Gates;

namespace BetterHades.Components
{
    public class Connection : IObserver<bool>, IObservable<bool>
    {
        private readonly ObservableCollection<IComponent> _inComponents;
        private readonly List<AndGate> _observers;
        private bool _isActive;

        public Connection()
        {
            _observers = new List<AndGate>();
            _inComponents = new ObservableCollection<IComponent>();
        }

        private bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                Notify(_isActive);
            }
        }

        public void OnCompleted() { Console.WriteLine("COMPLETED"); }

        public void OnError(Exception error) { Console.WriteLine(error); }

        public void OnNext(bool value)
        {
            IsActive = value;
            Console.WriteLine("NEXT " + value);
            foreach (var observer in _observers)
            {
                observer.Notify(value);
            }
        }

        public override int GetHashCode() { return IsActive.GetHashCode(); }

        public void Activate() { IsActive = true; }

        public void Deactivate() { IsActive = false; }

        public static bool operator ==(Connection connection, bool comparator)
        {
            return connection?.IsActive == comparator;
        }

        public static bool operator !=(Connection connection, bool comparator)
        {
            return connection?.IsActive != comparator;
        }

        private bool Equals(Connection other) { return IsActive == other.IsActive; }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            _observers.Add(observer as AndGate);
            return (observer as IDisposable)!;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Connection) obj);
        }

        private void Notify(bool b)
        {
            Console.WriteLine($"Connection NOTIFY {string.Join(" ", _observers)}");
            foreach (var obs in _observers) obs.OnNext(b);
        }

        public override string ToString() { return IsActive + ""; }
    }
}