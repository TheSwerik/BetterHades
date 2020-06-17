using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BetterHades.Components;
using BetterHades.Components.Implementations.Gates;
using BetterHades.Components.Implementations.IO;

namespace BetterHades
{
    public class MainWindow : Window
    {
        private readonly Canvas _canvas;
        private readonly List<Component> _components;
        private readonly List<Connection> _connections;
        private readonly List<Input> _inputs;
        private readonly List<Output> _outputs;

        public List<string> MyItems;
        private readonly ContextMenu rightClickCOntextMenu;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _inputs = new List<Input>();
            _outputs = new List<Output>();
            _components = new List<Component>();
            _connections = new List<Connection>();
            _canvas = (Canvas) LogicalChildren[0].LogicalChildren[0];
            _canvas.PointerPressed += ClickHandler;
            MyItems = new List<string> {"hello", "dishfsodif"};
            rightClickCOntextMenu = new ContextMenu {Background = Brushes.Aqua, Items = MyItems, IsVisible = false};
            rightClickCOntextMenu.PointerPressed += RightClickContextMenuSelection;
            rightClickCOntextMenu.KeyDown += RightClickContextMenuSelection;
            _canvas.Children.Add(rightClickCOntextMenu);
            // Test();
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void Test()
        {
            var checkbox1 = new CheckBox();
            checkbox1.Click += CheckboxOnClick;
            _canvas.Children.Add(checkbox1);
            var checkbox2 = new CheckBox();
            checkbox2.Click += CheckboxOnClick;
            _canvas.Children.Add(checkbox2);
            var rect1 = new TextBlock();
            rect1.Width = rect1.Height = 100;
            _canvas.Children.Add(rect1);
            var rect2 = new TextBlock();
            rect2.Width = rect2.Height = 100;
            _canvas.Children.Add(rect2);
            Canvas.SetTop(checkbox2, 100);
            Canvas.SetTop(rect1, -40);
            Canvas.SetTop(rect2, 60);
            Canvas.SetLeft(rect1, 100);
            Canvas.SetLeft(rect2, 100);

            _inputs.Add(new InputClock(checkbox1, 1000));
            _inputs.Add(new Input(checkbox2));
            _outputs.Add(new Output(rect1));
            _outputs.Add(new Output(rect2));
            var andGate = new ANDGate();
            _components.Add(andGate);
            _connections.Add(new Connection(_inputs[0], _outputs[0]));
            _connections.Add(new Connection(_inputs[0], andGate));
            _connections.Add(new Connection(_inputs[1], andGate));
            _connections.Add(new Connection(andGate, _outputs[1]));
        }

        public void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            _inputs.Find(i => i.InputBox.Equals(sender))?.Update();
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            var args = (PointerPressedEventArgs) e;
            var pos = args.GetCurrentPoint(_canvas).Position;
            if (args.MouseButton == MouseButton.Right)
            {
                Canvas.SetTop(rightClickCOntextMenu, pos.Y);
                Canvas.SetLeft(rightClickCOntextMenu, pos.X);
                rightClickCOntextMenu.IsVisible = true;
            }
            else if (args.MouseButton == MouseButton.Left)
            {
                // TODO
            }
        }

        private void RightClickContextMenuSelection(object sender, RoutedEventArgs e)
        {
            if (e is PointerPressedEventArgs mouseArgs && mouseArgs.MouseButton == MouseButton.Left || 
                e is KeyEventArgs keyArgs && keyArgs.Key == Key.Return)
            {
                Console.WriteLine(rightClickCOntextMenu.SelectedItem);
            }
        }
    }
}