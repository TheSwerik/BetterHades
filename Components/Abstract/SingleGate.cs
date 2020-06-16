using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BetterHades.Components
{
    public abstract class MultiGate : IObservingComponent
    {
        protected readonly ObservableCollection<Connection> _inputs;
        private bool _isActive;

        protected MultiGate()
        {
            _inputs = new ObservableCollection<Connection>();
            _inputs.CollectionChanged += Update;
        }

        public IDisposable Subscribe(IObserver<IComponent> observer) { throw new NotImplementedException(); }

        public void Update() { _isActive = Check(); }

        public void Notify(bool b) { throw new NotImplementedException(); }

        public bool IsActive() { throw new NotImplementedException(); }
        public void Update(Connection value) { Update(); }

        protected abstract bool Check();

        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }

        public void AddInput(Connection connection) { _inputs.Add(connection); }
    }
}