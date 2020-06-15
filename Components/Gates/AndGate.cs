using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BetterHades.Components.Gates
{
    public class AndGate : IComponent, IObservable<bool>, IObserver<bool>
    {
        private readonly ObservableCollection<Connection> _inConnections;
        private readonly List<IObserver<bool>> _observers;

        public AndGate()
        {
            _inConnections = new ObservableCollection<Connection>();
            _observers = new List<IObserver<bool>>();
            _inConnections.CollectionChanged += Check;
        }

        private void Check(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify(_inConnections.All(c => c == true));
        }

        public void AddInConnection(Connection c)
        {
            _inConnections.Add(c);
            c.Subscribe(this);
        }

        public override string ToString() { return _inConnections.All(c => c == true) + ""; }

        // Observable
        public IDisposable Subscribe(IObserver<bool> observer)
        {
            _observers.Add(observer);
            return (observer as IDisposable)!;
        }

        public void Notify(bool b)
        {
            Console.WriteLine("AND NOTIFY");
            foreach (var obs in _observers) obs.OnNext(b);
        }

        // Observer
        public void OnCompleted() { Console.WriteLine("COMPLETE"); }
        public void OnError(Exception error) { Console.WriteLine("EROORRR"); }
        public void OnNext(bool value) { Check(null, null); }
    }
}