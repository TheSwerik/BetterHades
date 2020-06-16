using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BetterHades.Components
{
    public abstract class Gate : Component, IObservingComponent
    {
        protected readonly ObservableCollection<Connection> Inputs;

        protected Gate()
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;
        }

        public void Update(Connection connection) { Update(); }

        public virtual void AddInput(Connection connection) { Inputs.Add(connection); }

        private void Update() { Notify(IsActive = Check()); }

        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }

        protected abstract bool Check();
    }
}