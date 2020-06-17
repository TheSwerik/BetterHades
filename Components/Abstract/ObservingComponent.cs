using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public abstract class ObservingComponent : Component, IObserver<Connection>
    {
        public readonly Point InPoint;
        protected readonly ObservableCollection<Connection> Inputs;

        protected ObservingComponent(IPanel parent, double x, double y, Point outPoint, Point inPoint) : base(
            parent, x, y, outPoint)
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;
            InPoint = inPoint;
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