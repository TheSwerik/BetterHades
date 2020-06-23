using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public abstract class ObservingComponent : Component, IObserver<Connection>
    {
        private readonly Ellipse InPointCircle;
        protected readonly ObservableCollection<Connection> Inputs;

        protected ObservingComponent(Point pos, bool isActive) : base(pos, isActive)
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;

            const double diameter = MainWindow.GridCellSize / 10.0;
            InPointCircle = new Ellipse {Fill = Brushes.Blue, Width = diameter, Height = diameter};
            App.MainWindow.GridCanvas.Canvas.Children.Add(InPointCircle);
            Canvas.SetTop(InPointCircle, InPoint.Y - diameter / 2);
            Canvas.SetLeft(InPointCircle, InPoint.X - diameter / 2);
        }

        public Point InPoint => Pos.WithX(Pos.X - MainWindow.GridCellSize);

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