using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BetterHades.Frontend;

namespace BetterHades
{
    public class MainWindow : Window
    {
        private readonly GridCanvas _canvas;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _canvas = new GridCanvas((DockPanel) LogicalChildren[0]);
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }
    }
}