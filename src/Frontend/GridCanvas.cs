using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.Visuals.Media.Imaging;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;
using BetterHades.Exceptions;
using BetterHades.Util;

namespace BetterHades.Frontend
{
    public class GridCanvas
    {
        private readonly Rectangle _previewComponent;
        private readonly string[] _previewComponentData;
        public readonly Canvas Canvas;
        public readonly List<Component> Components;
        public readonly List<Connection> Connections;
        private Component _buffer;
        private Component _moveBuffer;
        private Connection _removeConnection;
        private Polyline _previewConnection;
        public bool IsMoving;

        public GridCanvas(ZoomBorder parent)
        {
            Canvas = new Canvas
                     {
                         Background = Brushes.White,
                         Width = MainWindow.GridSize,
                         Height = MainWindow.GridSize
                     };
            Canvas.PointerMoved += MouseMoveHandler;
            Canvas.PointerPressed += ClickHandler;
            parent.Child = Canvas;

            DrawGrid();

            Dispatcher.UIThread.InvokeAsync
            (
                () =>
                {
                    parent.StartPan(0, 0);
                    parent.PanTo(-(MainWindow.GridSize - App.MainWindow.Width) / 2,
                                 -(MainWindow.GridSize - App.MainWindow.Height) / 2);
                },
                DispatcherPriority.Render
            );

            Components = new List<Component>();
            Connections = new List<Connection>();
            _previewComponentData = new string[2];
            Canvas.Children.Add(_previewComponent = new Rectangle
                                                    {
                                                        Width = 2 * MainWindow.GridCellSize,
                                                        Height = 2 * MainWindow.GridCellSize,
                                                        Stroke = Brushes.Black,
                                                        IsVisible = false,
                                                        StrokeThickness = 1
                                                    });
        }

        // Handlers:
        public void OnComponentClick(Component sender, PointerPressedEventArgs e)
        {
            if (_buffer == null)
            {
                _buffer = sender;
                var pos = ToGridCoordinates(e.GetCurrentPoint(Canvas).Position);
                Canvas.Children.Add(_previewConnection = new Polyline {Points = new List<Point> {pos, pos}});
            }
            else
            {
                if (!(sender is ObservingComponent))
                    Connections.Add(new Connection(sender, (ObservingComponent) _buffer, _previewConnection));
                else if (!(_buffer is Output))
                    Connections.Add(new Connection(_buffer, (ObservingComponent) sender, _previewConnection));
                _buffer = null;
                Canvas.Children.Remove(_previewConnection);
                _previewConnection = null;
                FileHandler.Changed();
            }
        }

        private void ClickHandler(object sender, PointerPressedEventArgs e)
        {
            var point = e.GetCurrentPoint(App.MainWindow);
            var pos = point.Position;
            if (point.Properties.IsRightButtonPressed) App.MainWindow.RightClickContextMenu.Show(pos.X, pos.Y);
            else if (point.Properties.IsLeftButtonPressed) App.MainWindow.RightClickContextMenu.Hide();

            point = e.GetCurrentPoint(Canvas);
            pos = ToGridCoordinates(point.Position);
            if (point.Properties.IsLeftButtonPressed)
            {
                if (_previewComponent.IsVisible)
                {
                    _previewComponent.IsVisible = false;
                    if (_moveBuffer != null)
                    {
                        _moveBuffer.MoveTo(pos);
                        _moveBuffer = null;
                        IsMoving = false;
                    }
                    else
                    {
                        AddComponent(_previewComponentData[0], _previewComponentData[1], pos);
                        _previewComponentData[0] = _previewComponentData[1] = null;
                    }

                    return;
                }

                if (_buffer == null)
                {
                    if (Components.Any(c => c.OutPoint == pos))
                    {
                        OnComponentClick(Components.First(c => c.OutPoint == pos), e);
                        return;
                    }

                    if (Components.Any(c => c is ObservingComponent oc && oc.InPoint == pos))
                    {
                        OnComponentClick(Components.First(c => c is ObservingComponent oc && oc.InPoint == pos), e);
                        return;
                    }
                }
                else if (Components.Any(c => c != _buffer && IsPointInShape(c.Polygon, pos)))
                {
                    OnComponentClick(Components.First(c => c != _buffer && IsPointInShape(c.Polygon, pos)), e);
                    return;
                }

                _previewConnection?.Points.Add(pos);
            }
            else if (point.Properties.IsRightButtonPressed)
            {
                _buffer = _moveBuffer = null;
                IsMoving = false;
                if (_previewConnection != null)
                {
                    Canvas.Children.Remove(_previewConnection);
                    _previewConnection = null;
                    App.MainWindow.RightClickContextMenu.Hide();
                }

                if (_previewComponent.IsVisible)
                {
                    _previewComponent.IsVisible = false;
                    _previewComponentData[0] = _previewComponentData[1] = null;
                    App.MainWindow.RightClickContextMenu.Hide();
                }


                if (Components.Any(c => c != _buffer && IsPointInShape(c.Polygon, pos)))
                    _moveBuffer = Components.First(c => c != _buffer && IsPointInShape(c.Polygon, pos));
                else if (Connections.Any(c => IsPointInShape(c.Polyline, pos)))
                    _removeConnection = Connections.First(c => IsPointInShape(c.Polyline, pos));
            }
        }

        private void MouseMoveHandler(object sender, PointerEventArgs e)
        {
            if (IsMoving && _moveBuffer != null) _previewComponent.IsVisible = true;
            var pos = ToGridCoordinates(e.GetCurrentPoint(Canvas).Position);
            if (_previewComponent.IsVisible)
            {
                Canvas.SetLeft(_previewComponent, pos.X - MainWindow.GridCellSize);
                Canvas.SetTop(_previewComponent, pos.Y - MainWindow.GridCellSize);
            }

            if (_previewConnection == null || _previewConnection.Points[^1] == pos) return;
            _previewConnection.Points[^1] = pos;
            Canvas.Children.Remove(_previewConnection);
            _previewConnection = new Polyline {Points = _previewConnection.Points, Stroke = Brushes.Black};
            Canvas.Children.Add(_previewConnection);
        }

        // Helper Methods:
        public void StartComponentPreview(string group, string type)
        {
            _previewComponentData[0] = group;
            _previewComponentData[1] = type;
            _previewComponent.IsVisible = true;
        }

        private void AddComponent(string group, string type, Point pos)
        {
            if (group.Equals("Gates")) type += "Gate";
            var t = Type.GetType($"BetterHades.Components.Implementations.{group}.{type}");
            if (t == null) throw new ComponentNotFoundException(type);
            Components.Add((Component) Activator.CreateInstance(t, pos, false) ??
                           throw new ComponentNotFoundException(type));
            FileHandler.Changed();
        }

        private void DrawGrid()
        {
            var bitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + @"res\Grid-small.png");
            const double width = 50.0 / MainWindow.GridCellSize;
            for (var i = 0; i < width * MainWindow.GridSize / 1000; i++)
            for (var j = 0; j < width * MainWindow.GridSize / 1000; j++)
            {
                var img = new Image {Source = bitmap, Width = 1000.0, Height = 1000.0};
                RenderOptions.SetBitmapInterpolationMode(img, BitmapInterpolationMode.Default);
                Canvas.Children.Add(img);
                Canvas.SetLeft(img, i * 1000 / width);
                Canvas.SetTop(img, j * 1000 / width);
            }
        }

        public static Point ToGridCoordinates(Point point)
        {
            return new Point(Math.Round(point.X / MainWindow.GridCellSize) * MainWindow.GridCellSize,
                             Math.Round(point.Y / MainWindow.GridCellSize) * MainWindow.GridCellSize);
        }

        private static bool IsPointInShape(Shape shape, Point point)
        {
            return (shape.DefiningGeometry?.FillContains(point) ?? throw new Exception("DefiningGeometry is null.")) ||
                   (shape.DefiningGeometry?.StrokeContains(new Pen(), point) ??
                    throw new Exception("DefiningGeometry is null."));
        }

        public void Remove()
        {
            if (_moveBuffer != null)
            {
                Components.Remove(_moveBuffer);
                _moveBuffer.Remove();
                _moveBuffer = null;
            }
            else if (_removeConnection != null)
            {
                _removeConnection.Remove();
                _removeConnection = null;
            }
        }
    }
}