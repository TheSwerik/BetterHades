using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BetterHades.Components;
using BetterHades.Components.Implementations.Gates;
using BetterHades.Components.Implementations.IO;

namespace BetterHades
{
    public class MainWindow : Window
    {
        private readonly List<Input> _inputs;
        private readonly List<Output> _outputs;
        private readonly List<Component> _components;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _inputs = new List<Input>();
            _outputs = new List<Output>();
            _components = new List<Component>();
            Test();
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        private void Test()
        {
            _inputs.Add(new Input((CheckBox) LogicalChildren[0].LogicalChildren[1]));
            _outputs.Add(new Output((TextBlock) LogicalChildren[0].LogicalChildren[2]));
        }

        public void CheckboxOnClick(object sender, RoutedEventArgs e)
        {
            _inputs.Find(i => i.InputBox.Equals(sender))?.Update();
        }
    }
}