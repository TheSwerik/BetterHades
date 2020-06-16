using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BetterHades.Components
{
    public abstract class Gate : Input, IObserver<Connection>
    {
        protected readonly ObservableCollection<Connection> InConnections;

        protected Gate()
        {
            InConnections = new ObservableCollection<Connection>();
            InConnections.CollectionChanged += Update;
        }

        // Observer
        public void OnNext(Connection value) { Update(); }
        public void OnCompleted() { Console.WriteLine("GATE COMPLETE"); }
        public void OnError(Exception error) { Console.WriteLine("GATE " + error); }

        // Helper Methods
        public void AddInConnection(Connection c)
        {
            InConnections.Add(c);
            // c.Subscribe(this);
        }
    }
}