using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;
using BetterHades.Util;

namespace BetterHades.Frontend
{
    public class GridCanvas
    {
        private readonly ZoomBorder _zoomBorder;
        public readonly Canvas Canvas;
        public readonly List<Component> Components;
        public readonly List<Connection> Connections;
        private Component _buffer;

        public GridCanvas(ZoomBorder parent)
        {
            _zoomBorder = parent;
            Canvas = new Canvas
                     {
                         Background = Brushes.White,
                         Width = MainWindow.GridSize,
                         Height = MainWindow.GridSize
                     };
            DrawGrid();

            _zoomBorder.Child = Canvas;
            Dispatcher.UIThread.InvokeAsync
            (
                () =>
                {
                    _zoomBorder.StartPan(0, 0);
                    _zoomBorder.PanTo(-(MainWindow.GridSize - App.MainWindow.Width) / 2,
                                      -(MainWindow.GridSize - App.MainWindow.Height) / 2);
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

        private void DrawGrid()
        {
            var bitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + @"res\Grid-small.png");
            //TODO finalize grid
            for (var i = 0; i < MainWindow.GridSize / 1000; i++)
            {
                for (var j = 0; j < MainWindow.GridSize / 1000; j++)
                {
                    var img = new Image
                              {
                                  Source = bitmap,
                                  Stretch = Stretch.None,
                                  UseLayoutRounding = true,
                              };
                    
                    Canvas.Children.Add(img);
                    Avalonia.Controls.Canvas.SetLeft(img, i * 1000);
                    Avalonia.Controls.Canvas.SetTop(img, j * 1000);
                }
            }

            // Canvas.Children.Add(new Image {Source = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + @"res\Grid2.png")});

            // var points = new List<Line>();
            // for (var i = 0; i <= MainWindow.GridSize; i += MainWindow.GridCellSize)
            // for (var j = 0; j <= MainWindow.GridSize; j += MainWindow.GridCellSize)
            // {
            //     var p = new Point(i, j);
            //     points.Add(new Line() {Stroke = Brushes.Gray, StartPoint = p, EndPoint = p.WithX(p.X+1)});
            // }
            //
            // Canvas.Children.AddRange(points);
        }
    }
}