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
        private readonly Canvas _canvas;
        private readonly List<Component> _components;
        private readonly List<Connection> _connections;
        private readonly RightClickContextMenu _contextMenu;
        private readonly List<Input> _inputs;
        private readonly List<Output> _outputs;

        public GridCanvas(IPanel parent)
        {
            _canvas = new Canvas {Background = Brushes.LightGray};
            _canvas.PointerPressed += ClickHandler;
            parent.Children.Add(_canvas);
            _contextMenu = new RightClickContextMenu(_canvas, this);
            _inputs = new List<Input>();
            _outputs = new List<Output>();
            _components = new List<Component>();
            _connections = new List<Connection>();
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            var args = (PointerPressedEventArgs) e;
            var pos = args.GetCurrentPoint(_canvas).Position;
            if (args.MouseButton == MouseButton.Right)
            {
                _contextMenu.Show(pos.X, pos.Y);
            }
            else if (args.MouseButton == MouseButton.Left)
            {
                // TODO
            }
        }

        public void AddComponent(string group, string type)
        {
            if (group.Equals("Gates")) type += "Gate";
            var t = System.Type.GetType($"BetterHades.Components.Implementations.{group}.{type}");
            if (t == null) throw new ComponentNotFoundException(type);
            // TODO Same Constructor for all
            Component component;
            if (group.Equals("IO"))
                component = (Component) Activator.CreateInstance(t, _canvas) ??
                            throw new ComponentNotFoundException(type);
            else component = (Component) Activator.CreateInstance(t) ?? throw new ComponentNotFoundException(type);
            _components.Add(component);
        }
    }
}