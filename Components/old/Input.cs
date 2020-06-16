using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BetterHades.Components
{
    public abstract class Input : IObservable<Gate>
    {
        private readonly List<Connection> _outConnections;
        private bool _isActive;

        protected Input()
        {
            _outConnections = new List<Connection>();
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value) Notify(_isActive = value);
                else _isActive = value;
            }
        }

        // Observable
        public IDisposable Subscribe(IObserver<Gate> observerConnection)
        {
            _outConnections.Add(observerConnection as Connection);
            return (observerConnection as IDisposable)!;
        }

        // Output Methods
        protected abstract bool Check();
        protected void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }
        protected void Update() { IsActive = Check(); }

        private void Notify(bool b)
        {
            foreach (var obs in _outConnections) obs.OnNext(this);
        }

        // Helper Methods
        public override string ToString() { return IsActive + ""; }
    }
}