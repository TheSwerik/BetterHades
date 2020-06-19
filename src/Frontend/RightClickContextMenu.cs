using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using BetterHades.Components;

namespace BetterHades.Frontend
{
    public class RightClickContextMenu
    {
        private readonly ContextMenu _contextMenu;

        public RightClickContextMenu(ContextMenu contextMenu)
        {
            _contextMenu = contextMenu;
            _contextMenu.Items = MenuItems();
            _contextMenu.PointerPressed += OnClick;
            _contextMenu.KeyDown += OnClick;
            Hide();
        }

        // Handlers:
        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (!(e is PointerPressedEventArgs mouseArgs &&
                  mouseArgs.GetCurrentPoint(App.MainWindow).Properties.IsLeftButtonPressed ||
                  e is KeyEventArgs keyArgs && keyArgs.Key == Key.Return)) return;
            var selected = (MenuItem) _contextMenu.SelectedItem;
            var group = (string) selected.Header;
            var translatedPoint = _contextMenu.Parent.TranslatePoint(
                new Point(Canvas.GetLeft(_contextMenu), Canvas.GetTop(_contextMenu)),
                App.MainWindow.GridCanvas.Canvas
            )!.Value;
            App.MainWindow.GridCanvas.AddComponent(
                group,
                selected.SelectedItem.ToString(),
                Math.Round(translatedPoint.X / MainWindow.GridCellSize) * MainWindow.GridCellSize,
                Math.Round(translatedPoint.Y / MainWindow.GridCellSize) * MainWindow.GridCellSize
            );
            Hide();
        }

        // Public Methods:
        public void Show(in double posX, in double posY)
        {
            Canvas.SetLeft(_contextMenu, posX);
            Canvas.SetTop(_contextMenu, posY);
            _contextMenu.IsVisible = true;
            foreach (MenuItem item in _contextMenu.Items) item.IsVisible = true;
        }

        public void Hide()
        {
            _contextMenu.IsVisible = false;
            ((MenuItem) _contextMenu.SelectedItem)?.Close();
            Canvas.SetLeft(_contextMenu, 0);
            Canvas.SetTop(_contextMenu, 0);
        }

        // Helper Methods:
        private static IEnumerable<MenuItem> MenuItems()
        {
            return Component.ToDictionary()
                            .Select(group => new MenuItem {Header = group.Key, Items = group.Value})
                            .ToList();
        }
    }
}