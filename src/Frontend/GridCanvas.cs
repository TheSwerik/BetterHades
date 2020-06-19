using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;
using BetterHades.Util;
using SkiaSharp;
using Component = BetterHades.Components.Component;

namespace BetterHades.Frontend
{
    public class GridCanvas
    {
        private readonly ZoomBorder _zoomBorder;
        public readonly Canvas Canvas;
        public readonly List<Component> Components;
        public readonly List<Connection> Connections;
        private Component _buffer;
        public const int Width = 5000;

        public GridCanvas(ZoomBorder parent)
        {
            _zoomBorder = parent;
            Canvas = new Canvas {Background = Brushes.LightGray, Width = Width, Height = Width,};
            for (var i = 100; i < Width; i += 100)
            for (var j = 100; j < Width; j += 100)
                Canvas.Children.Add(Cross(i, j));


            _zoomBorder.Child = Canvas;
            Dispatcher.UIThread.InvokeAsync
            (
                () =>
                {
                    _zoomBorder.StartPan(0,0);
                    _zoomBorder.PanTo(-(Width - App.MainWindow.Width) / 2, -(Width - App.MainWindow.Height) / 2);
                },
                DispatcherPriority.Render
            );
            Components = new List<Component>();
            Connections = new List<Connection>();
        }

        // Handlers:

        public void OnComponentInClick(ObservingComponent sender)
        {
            if (_buffer == null)
            {
                _buffer = sender;
            }
            else
            {
                if (!(_buffer is Output)) Connections.Add(new Connection(_buffer, sender, Canvas));
                _buffer = null;
                FileHandler.Changed();
            }
        }

        public void OnComponentOutClick(Component sender)
        {
            if (_buffer == null)
            {
                _buffer = sender;
            }
            else
            {
                Connections.Add(new Connection(sender, _buffer as ObservingComponent, Canvas));
                _buffer = null;
                FileHandler.Changed();
            }
        }

        // Helper Methods:
        public void AddComponent(string group, string type, double x, double y)
        {
            if (group.Equals("Gates")) type += "Gate";
            var t = Type.GetType($"BetterHades.Components.Implementations.{group}.{type}");
            if (t == null) throw new ComponentNotFoundException(type);
            Components.Add((Component) Activator.CreateInstance(t, this, x, y, false) ??
                           throw new ComponentNotFoundException(type));
            FileHandler.Changed();
        }

        private Polyline Cross(double x, double y)
        {
            var middle = new Point(x, y);
            return new Polyline()
                   {
                       Stroke = Brushes.Black,
                       Points = new List<Point>()
                                {
                                    middle,
                                    middle.WithX(x + 10),
                                    middle,
                                    middle.WithX(x - 10),
                                    middle,
                                    middle.WithY(y + 10),
                                    middle,
                                    middle.WithY(y - 10),
                                }
                   };
        }
    }
}