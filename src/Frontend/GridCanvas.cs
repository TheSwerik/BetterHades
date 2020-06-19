using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;
using BetterHades.Util;

namespace BetterHades.Frontend
{
    public class GridCanvas
    {
        private readonly RightClickContextMenu _contextMenu;
        public readonly Canvas Canvas;
        public readonly List<Component> Components;
        public readonly List<Connection> Connections;
        private Component _buffer;

        public GridCanvas(IPanel parent)
        {
            Canvas = new Canvas {Background = Brushes.LightGray};
            Canvas.PointerPressed += ClickHandler;
            parent.Children.Add(Canvas);
            _contextMenu = new RightClickContextMenu(Canvas, this);
            Components = new List<Component>();
            Connections = new List<Connection>();
        }

        // Handlers:
        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            var args = (PointerPressedEventArgs) e;
            var pos = args.GetCurrentPoint(Canvas).Position;
            if (args.MouseButton == MouseButton.Right) _contextMenu.Show(pos.X, pos.Y);
            else if (args.MouseButton == MouseButton.Left) _contextMenu.Hide();
        }

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
    }
}