using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Controls;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public abstract class ObservingComponent : Component, IObserver<Connection>
    {
        protected readonly ObservableCollection<Connection> Inputs;

        protected ObservingComponent(IPanel parent, double x, double y) : base(parent, x, y)
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;
        }

        // Implemented:
        public void OnCompleted() { throw new CompletedException(); }
        public void OnError(Exception error) { Console.WriteLine(error); }
        public void OnNext(Connection value) { Update(); }

        // Abstract:
        public virtual void AddInput(Connection connection) { Inputs.Add(connection); }
        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }
        protected abstract void Update();
    }
}