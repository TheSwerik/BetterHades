﻿using System;
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
        public readonly Ellipse InPoint;
        protected readonly ObservableCollection<Connection> Inputs;

        protected ObservingComponent(double x, double y, bool isActive, Point outPoint, Point inPoint)
            : base(x, y, isActive, outPoint)
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;

            const double diameter = MainWindow.GridCellSize / 10.0;
            InPoint = new Ellipse {Fill = Brushes.Blue, Width = diameter, Height = diameter};
            App.MainWindow.GridCanvas.Canvas.Children.Add(InPoint);
            Canvas.SetTop(InPoint, y - diameter / 2);
            Canvas.SetLeft(InPoint, x - diameter / 2);
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