using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BetterHades.Components;
using BetterHades.Components.Gates;

namespace BetterHades
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Test();
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void Test()
        {
            var and = new AndGate();
            var c1 = new Connection();
            var c2 = new Connection();
            var c3 = new Connection();
            and.AddInConnection(c1);
            and.AddInConnection(c2);
            and.Subscribe(c3);
            c1.Activate();
            c2.Activate();
            c2.Deactivate();
        }
    }
}