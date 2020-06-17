using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
            // this.AttachDevTools();
#endif
            _canvas = new GridCanvas((DockPanel) LogicalChildren[0]);
        }

        private void InitializeComponent() { AvaloniaXamlLoader.Load(this); }

        public void AboutOnClick(object sender, RoutedEventArgs args)
        {
            var window = new Window
                         {
                             Title = "About",
                             CanResize = false,
                             ShowInTaskbar = false,
                             Width = 300,
                             Height = 100,
                             WindowStartupLocation = WindowStartupLocation.CenterOwner,
                             Content = string.Join("\n", File.ReadLines(@"Resources\about.txt"))
                         };
            window.ShowDialog(App.MainWindow);
        }
    }
}