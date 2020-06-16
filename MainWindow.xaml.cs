using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BetterHades.Components;
using BetterHades.Components.Gates;
using BetterHades.Components.IO;
using Serilog;
using SharpDX.Direct2D1;

namespace BetterHades
{
    public class MainWindow : Window
    {
        private TextBlock _outBox;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _outBox = ((TextBlock) this.LogicalChildren[0].LogicalChildren[2]);
            inputs = new List<InputImpl>();
            Test();
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }
        private List<InputImpl> inputs;
        private AndGate and;

        private void Test()
        {
            and = new AndGate();
            inputs.Add(new InputImpl());
            inputs.Add(new InputImpl());
            var c1 = new Connection(inputs[0], and);
            var c2 = new Connection(inputs[1], and);
            // var c3 = new Connection(and,);
            and.AddInConnection(c1);
            and.AddInConnection(c2);
            // and.Subscribe(c3);
            // Console.WriteLine($"Gate {and} --- Connection {c3}");
            // c1.Activate();
            // Console.WriteLine($"Gate {and} --- Connection {c3}");
            // c2.Activate();
            // Console.WriteLine($"Gate {and} --- Connection {c3}");
            // c2.Deactivate();
            // Console.WriteLine($"Gate {and} --- Connection {c3}");
        }

        public void button_Click(object sender, RoutedEventArgs e)
        {
            var senderCheckbox = ((CheckBox) sender);
            var isChecked = senderCheckbox.IsChecked ?? false;
            var index = int.Parse(Regex.Replace(senderCheckbox.Name, @"[^\d]", ""));
            inputs[index - 1].IsActive = isChecked;
            _outBox.Background = and.IsActive ? Brushes.Red : Brushes.Gray;
        }
    }
}