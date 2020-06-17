using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;

namespace BetterHades.Components
{
    public abstract class Gate : Component, IObservingComponent
    {
        protected readonly ObservableCollection<Connection> Inputs;

        protected Gate(IPanel parent, double x, double y) : base(parent, x, y)
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;
        }

        public void Update(Connection connection) { Update(); }

        public virtual void AddInput(Connection connection) { Inputs.Add(connection); }

        private void Update() { Notify(IsActive = Check()); }

        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }

        protected abstract bool Check();

        protected override List<Point> GetPoints(double x, double y)
        {
            return new List<Point>
                   {
                       new Point(x - 5, y - 5),
                       new Point(x + 5, y - 5),
                       new Point(x - 5, y + 5),
                       new Point(x + 5, y + 5)
                   };
        }
    }
}