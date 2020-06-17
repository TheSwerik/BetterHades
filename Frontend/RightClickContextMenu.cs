using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using BetterHades.Components;

namespace BetterHades.Frontend
{
    public class RightClickContextMenu
    {
        private readonly ContextMenu _contextMenu;
        private readonly GridCanvas _canvas;

        public RightClickContextMenu(IPanel parent, GridCanvas canvas)
        {
            _contextMenu = new ContextMenu
                           {
                               Background = Brushes.Aqua,
                               Items = MenuItems(),
                               IsVisible = false
                           };
            _contextMenu.PointerPressed += OnClick;
            _contextMenu.KeyDown += OnClick;
            parent.Children.Add(_contextMenu);
            _canvas = canvas;
            Hide();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (!((e is PointerPressedEventArgs mouseArgs) && mouseArgs.MouseButton == MouseButton.Left ||
                  (e is KeyEventArgs keyArgs) && keyArgs.Key == Key.Return)) return;
            var selected = ((MenuItem) _contextMenu.SelectedItem);
            var group = (string) selected.Header;
            _canvas.AddComponent(@group, (Component.Type) selected.SelectedItem);
        }

        public void Show(in double posX, in double posY)
        {
            Canvas.SetLeft(_contextMenu, posX);
            Canvas.SetTop(_contextMenu, posY);
            _contextMenu.IsVisible = true;
            _contextMenu.ZIndex = _contextMenu.Parent.ZIndex + 1;
        }

        public void Hide()
        {
            _contextMenu.IsVisible = false;
            Canvas.SetLeft(_contextMenu, 0);
            Canvas.SetTop(_contextMenu, 0);
        }

        private static IEnumerable<MenuItem> MenuItems()
        {
            return Component.ToDictionary()
                            .Select(@group => new MenuItem {Header = @group.Key, Items = @group.Value})
                            .ToList();
        }
    }
}