using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BetterHades.Components
{
    public abstract class Gate : IObservable<Gate>, IObserver<Connection>
    {
        private readonly List<Connection> _outConnections;
        protected readonly ObservableCollection<Connection> InConnections;
        private bool _output;

        protected Gate()
        {
            InConnections = new ObservableCollection<Connection>();
            _outConnections = new List<Connection>();
            InConnections.CollectionChanged += Update;
        }

        public bool Output
        {
            get => _output;
            set
            {
                if (_output != value) Notify(_output = value);
                else _output = value;
            }
        }

        // Observable
        public IDisposable Subscribe(IObserver<Gate> observerConnection)
        {
            _outConnections.Add(observerConnection as Connection);
            return (observerConnection as IDisposable)!;
        }

        // Observer
        public void OnNext(Connection value) { Update(); }
        public void OnCompleted() { Console.WriteLine("GATE COMPLETE"); }
        public void OnError(Exception error) { Console.WriteLine("GATE " + error); }

        // Output Methods
        protected abstract bool Check();
        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }
        private void Update() { Output = Check(); }

        private void Notify(bool b)
        {
            foreach (var obs in _outConnections) obs.OnNext(this);
        }

        // Helper Methods
        public void AddInConnection(Connection c)
        {
            InConnections.Add(c);
            c.Subscribe(this);
        }

        public override string ToString() { return InConnections.All(c => c == true) + ""; }
    }
}