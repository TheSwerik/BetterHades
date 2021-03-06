﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using BetterHades.Exceptions;

namespace BetterHades.Components
{
    public abstract class ObservingComponent : Component, IObserver<Connection>
    {
        public readonly ObservableCollection<Connection> Inputs;
        private Ellipse InPointCircle;

        protected ObservingComponent(Point pos, bool isActive, string text) : base(pos, isActive, text)
        {
            Inputs = new ObservableCollection<Connection>();
            Inputs.CollectionChanged += Update;
            InPointCircle = GenerateIOPort(InPoint, Brushes.Orange);
        }

        public Point InPoint => Pos.WithX(Pos.X - MainWindow.GridCellSize);
        public int NumberOfInputs => Inputs.Count;

        // Implemented:
        public void OnCompleted() { throw new CompletedException(); }
        public void OnError(Exception error) { Console.WriteLine(error); }
        public void OnNext(Connection value) { Update(); }

        public override void MoveTo(Point pos)
        {
            var oldIn = InPoint;
            base.MoveTo(pos);
            App.MainWindow.GridCanvas.Canvas.Children.Remove(InPointCircle);
            InPointCircle = GenerateIOPort(InPoint, Brushes.Orange);
            foreach (var c in Inputs) c.UpdateLine(oldIn, InPoint);
        }

        public override void Remove()
        {
            base.Remove();
            for (var i = 0; i < Inputs.Count; i++) Inputs[i--].Remove();
            App.MainWindow.GridCanvas.Canvas.Children.Remove(InPointCircle);
        }

        public override void Remove(Connection connection)
        {
            base.Remove(connection);
            if (Inputs.Contains(connection)) Inputs.Remove(connection);
        }

        // Abstract:
        public virtual void AddInput(Connection connection) { Inputs.Add(connection); }
        private void Update(object sender, NotifyCollectionChangedEventArgs e) { Update(); }
        protected abstract void Update();
    }
}