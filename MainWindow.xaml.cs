﻿using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BetterHades.Components;
using BetterHades.Components.Implementations.IO;

namespace BetterHades
{
    public class MainWindow : Window
    {
        private readonly List<Component> _components;
        private readonly List<Connection> _connections;
        private readonly List<Input> _inputs;
        private readonly List<Output> _outputs;

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
            Test();
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void Test()
        {
            _inputs.Add(new Input((CheckBox) LogicalChildren[0].LogicalChildren[0]));
            _inputs.Add(new Input((CheckBox) LogicalChildren[0].LogicalChildren[1]));
            _outputs.Add(new Output((TextBlock) LogicalChildren[0].LogicalChildren[2]));
            _connections.Add(new Connection(_inputs[0], _outputs[0]));
        }

        public void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            _inputs.Find(i => i.InputBox.Equals(sender))?.Update();
        }
    }
}