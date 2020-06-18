using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public readonly Canvas Canvas;
        private readonly List<Component> _components;
        private readonly List<Connection> _connections;
        private readonly RightClickContextMenu _contextMenu;
        private readonly List<Input> _inputs;
        private readonly List<Output> _outputs;

        public GridCanvas(IPanel parent)
        {
            Canvas = new Canvas {Background = Brushes.LightGray};
            Canvas.PointerPressed += ClickHandler;
            parent.Children.Add(Canvas);
            _contextMenu = new RightClickContextMenu(Canvas, this);
            _inputs = new List<Input>();
            _outputs = new List<Output>();
            _components = new List<Component>();
            _connections = new List<Connection>();
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            var args = (PointerPressedEventArgs) e;
            var pos = args.GetCurrentPoint(Canvas).Position;
            if (args.MouseButton == MouseButton.Right)
            {
                _contextMenu.Show(pos.X, pos.Y);
            }
            else if (args.MouseButton == MouseButton.Left)
            {
                _contextMenu.Hide();
            }
        }

        private Component buffer;

        public void OnComponentInClick(ObservingComponent sender)
        {
            if (buffer == null) buffer = sender;
            else
            {
                if (!(buffer is Output)) _connections.Add(new Connection(buffer, sender, Canvas));
                buffer = null;
            }
        }

        public void OnComponentOutClick(Component sender)
        {
            if (buffer == null) buffer = sender;
            else
            {
                _connections.Add(new Connection(sender, buffer as ObservingComponent, Canvas));
                buffer = null;
            }
        }

        public void AddComponent(string group, string type, double x, double y)
        {
            if (group.Equals("Gates")) type += "Gate";
            var t = Type.GetType($"BetterHades.Components.Implementations.{group}.{type}");
            if (t == null) throw new ComponentNotFoundException(type);
            _components.Add((Component) Activator.CreateInstance(t, this, x, y) ??
                            throw new ComponentNotFoundException(type));
        }
    }
}