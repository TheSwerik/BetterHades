﻿using System;
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
        private readonly string[] _previewComponentData;
        private readonly ZoomBorder _zoomBorder;
        public readonly Canvas Canvas;
        public readonly List<Component> Components;
        public readonly List<Connection> Connections;
        private Component _buffer;
        private Rectangle _previewComponent;
        private Polyline _previewConnection;

        public GridCanvas(ZoomBorder parent)
        {
            _zoomBorder = parent;
            Canvas = new Canvas
                     {
                         Background = Brushes.White,
                         Width = MainWindow.GridSize,
                         Height = MainWindow.GridSize
                     };
            Canvas.PointerMoved += MoveHandler;
            Canvas.PointerPressed += ClickHandler;
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
            _previewComponentData = new string[2];
            _previewComponent = new Rectangle
                                {
                                    Width = 3 * MainWindow.GridCellSize,
                                    Height = 3 * MainWindow.GridCellSize,
                                    Stroke = Brushes.Black,
                                    IsVisible = false
                                };
            Canvas.Children.Add(_previewComponent);
            Components = new List<Component>();
            Connections = new List<Connection>();
        }

        // Handlers:
        public void OnComponentClick(Component sender, PointerPressedEventArgs e)
        {
            if (_buffer == null)
            {
                _buffer = sender;
                _previewConnection =
                    new Polyline {Points = new List<Point> {ToGridCoordinates(e.GetCurrentPoint(Canvas).Position)}};
                Canvas.Children.Add(_previewConnection);
            }
            else
            {
                if (sender is ObservingComponent observingSender)
                {
                    if (!(_buffer is Output))
                        Connections.Add(new Connection(_buffer, observingSender, _previewConnection));
                }
                else
                {
                    Connections.Add(new Connection(sender, _buffer as ObservingComponent, _previewConnection));
                }

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
                    AddComponent(_previewComponentData[0], _previewComponentData[1], pos);
                    _previewComponentData[0] = _previewComponentData[1] = null;
                    return;
                }

                if (Components.Any(c => c != _buffer && c.OutPoint == pos))
                {
                    OnComponentClick(Components.First(c => c != _buffer && c.OutPoint == pos), e);
                    return;
                }

                if (Components.Any(c => c != _buffer && c is ObservingComponent oc && oc.InPoint == pos))
                {
                    OnComponentClick(
                        Components
                            .First(c => c != _buffer && c is ObservingComponent oc && oc.InPoint == pos)
                        , e);
                    return;
                }

                _previewConnection?.Points.Add(pos);
            }
            else if (point.Properties.IsRightButtonPressed)
            {
                _buffer = null;
                if (_previewConnection != null)
                {
                    Canvas.Children.Remove(_previewConnection);
                    _previewConnection = null;
                    App.MainWindow.RightClickContextMenu.Hide();
                }
            }
        }

        private void MoveHandler(object sender, PointerEventArgs e)
        {
            var pos = ToGridCoordinates(e.GetCurrentPoint(Canvas).Position);
            if (_previewComponent.IsVisible)
            {
                Canvas.Children.Remove(_previewComponent);
                _previewComponent = new Rectangle
                                    {
                                        Width = 3 * MainWindow.GridCellSize,
                                        Height = 3 * MainWindow.GridCellSize,
                                        Stroke = Brushes.Black
                                    };
                Canvas.Children.Add(_previewComponent);
                Canvas.SetLeft(_previewComponent, pos.X - MainWindow.GridCellSize);
                Canvas.SetTop(_previewComponent, pos.Y - MainWindow.GridCellSize);
                Console.WriteLine(_previewComponent);
            }

            if (_previewConnection == null || _previewConnection.Points[^1] == pos) return;
            _previewConnection.Points[^1] = pos;
            Canvas.Children.Remove(_previewConnection);
            _previewConnection = new Polyline {Points = _previewConnection.Points, Stroke = Brushes.Black};
            Canvas.Children.Add(_previewConnection);
        }

        // Helper Methods:
        public void StartComponentPriview(string group, string type)
        {
            _previewComponentData[0] = group;
            _previewComponentData[1] = type;
            _previewComponent.IsVisible = true;
        }

        public void AddComponent(string group, string type, Point pos)
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

        public static double ToGridCoordinates(double value)
        {
            return Math.Round(value / MainWindow.GridCellSize) * MainWindow.GridCellSize;
        }

        public static Point ToGridCoordinates(Point point)
        {
            return new Point(ToGridCoordinates(point.X), ToGridCoordinates(point.Y));
        }
    }
}