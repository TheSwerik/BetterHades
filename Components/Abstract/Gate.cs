using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BetterHades.Components
{
    public abstract class Gate : MultiOutputComponent, IObservingComponent
    {
        protected readonly ObservableCollection<Connection> Inputs;

        protected Gate()
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;
        }

        public override void Update()
        {
            SetActive(Check());
            Notify(IsActive());
        }

        public void Update(Connection value) { Update(); }
        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }

        public void AddInput(Connection connection) { Inputs.Add(connection); }
        protected abstract bool Check();
    }
}