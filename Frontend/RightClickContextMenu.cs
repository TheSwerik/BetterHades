using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace BetterHades.Frontend
{
    public class RightClickContextMenu
    {
        private readonly ContextMenu _contextMenu;
        private readonly List<Component> _myItems;

        public RightClickContextMenu(IPanel parent)
        {
            _myItems = Enum.GetValues(typeof(Component)).Cast<Component>().ToList();
            _contextMenu = new ContextMenu {Background = Brushes.Aqua, Items = _myItems, IsVisible = false};
            _contextMenu.PointerPressed += OnClick;
            _contextMenu.KeyDown += OnClick;
            parent.Children.Add(_contextMenu);
            Hide();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (e is PointerPressedEventArgs mouseArgs && mouseArgs.MouseButton == MouseButton.Left ||
                e is KeyEventArgs keyArgs && keyArgs.Key == Key.Return)
                Console.WriteLine(_contextMenu.SelectedItem);
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

        private enum Component
        {
            AND,
            OR,
            Input,
            Output
        }
    }
}