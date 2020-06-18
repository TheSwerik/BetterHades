using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;

namespace BetterHades.Frontend
{
    public class GridCanvas
    {
        private readonly RightClickContextMenu _contextMenu;
        public readonly Canvas Canvas;
        public readonly List<Component> Components;
        public readonly List<Connection> Connections;

        private Component buffer;

        public GridCanvas(IPanel parent) : this(parent, new List<Component>(), new List<Connection>()) { }

        public GridCanvas(IPanel parent, List<Component> components, List<Connection> connections)
        {
            Canvas = new Canvas {Background = Brushes.LightGray};
            Canvas.PointerPressed += ClickHandler;
            parent.Children.Add(Canvas);
            _contextMenu = new RightClickContextMenu(Canvas, this);
            Components = components;
            Connections = connections;
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(string.Join("\n", Connections));
            var args = (PointerPressedEventArgs) e;
            var pos = args.GetCurrentPoint(Canvas).Position;
            if (args.MouseButton == MouseButton.Right) _contextMenu.Show(pos.X, pos.Y);
            else if (args.MouseButton == MouseButton.Left) _contextMenu.Hide();
        }

        public void OnComponentInClick(ObservingComponent sender)
        {
            if (buffer == null)
            {
                buffer = sender;
            }
            else
            {
                if (!(buffer is Output)) Connections.Add(new Connection(buffer, sender, Canvas));
                buffer = null;
            }
        }

        public void OnComponentOutClick(Component sender)
        {
            if (buffer == null)
            {
                buffer = sender;
            }
            else
            {
                Connections.Add(new Connection(sender, buffer as ObservingComponent, Canvas));
                buffer = null;
            }
        }

        public void AddComponent(string group, string type, double x, double y)
        {
            if (group.Equals("Gates")) type += "Gate";
            var t = Type.GetType($"BetterHades.Components.Implementations.{group}.{type}");
            if (t == null) throw new ComponentNotFoundException(type);
            Components.Add((Component) Activator.CreateInstance(t, this, x, y, false) ??
                           throw new ComponentNotFoundException(type));
        }
    }
}