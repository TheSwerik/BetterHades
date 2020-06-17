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
                while (_components.Count(c => c.IsClicked) >= 2)
                {
                    var inComponent = _components.First(c => c.IsClicked && !(c is ObservingComponent));
                    if (inComponent == null) break;
                    var outComponent = (ObservingComponent) _components.First(c => c.IsClicked && c is ObservingComponent);
                    if (outComponent == null) break;
                    inComponent.IsClicked = false;
                    outComponent.IsClicked = false;
                    _connections.Add(new Connection(inComponent, outComponent));
                }
            }
        }

        public void AddComponent(string group, string type, double x, double y)
        {
            if (group.Equals("Gates")) type += "Gate";
            var t = Type.GetType($"BetterHades.Components.Implementations.{group}.{type}");
            if (t == null) throw new ComponentNotFoundException(type);
            _components.Add((Component) Activator.CreateInstance(t, _canvas, x, y) ??
                            throw new ComponentNotFoundException(type));
        }
    }
}